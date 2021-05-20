from argparse import ArgumentParser
from os.path import isfile
from sys import argv
from myfacenet.detector import MTCNNDetector
from myfacenet.encoder import FacenetEncoder
from myfacenet.indentifier_faiss import FaissIdentifier
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from cv2 import rectangle, putText, FONT_HERSHEY_COMPLEX_SMALL
from cv2 import imread, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows, imwrite

FACENET_MODEL = 'models/premodels/512/20180402-114759.pb'
INDEXING_MODEL = 'models/mymodels/1814_140.index'
MTCNN_MODEL = 'models/premodels/align'
HAARCASCADE_MODEL = 'models/premodels/haarcascade/haarcascade_frontalface_default.xml'
THRESHOLD = 0.5
GPU_MEM_FRACTION = 0.25
FACE_SIZE = 140
MIN_SIZE = 20


def main(args):
    if isfile(args.image):
        with Graph().as_default():
            gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
            sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

            with sess.as_default():
                detector = MTCNNDetector(sess, MTCNN_MODEL, MIN_SIZE)
                encoder = FacenetEncoder(FACENET_MODEL, FACE_SIZE)
                identifier = FaissIdentifier(None, INDEXING_MODEL)

                namedWindow('image', WINDOW_NORMAL)

                img = imread(args.image)

                faces = detector.detect(img)

                count = 0
                for face in faces:
                    x1 = int(face[0])
                    y1 = int(face[1])
                    x2 = int(face[2])
                    y2 = int(face[3])

                    imwrite(f'output/face_{count}.jpg', img[y1:y2, x1:x2])
                    count += 1

                    face_embedding = encoder.encode_face(sess, img[y1:y2, x1:x2])
                    distance, student_id = identifier.identify(face_embedding)
                    distance = round(distance, 2)

                    if distance < THRESHOLD:
                        put_face_label(img, x1, y1, x2, y2, student_id, distance)
                    else:
                        put_face_label(img, x1, y1, x2, y2, 'Unknown', 'Nan')

                    print(f'Id: {student_id}, confidence: {distance}')

                imshow('image', img)

                if waitKey(0) & 0xFF == ord('q'):
                    destroyAllWindows()

    else:
        print('File does not exits')


def put_face_label(frame, x1, y1, x2, y2, student_id, distance):
    rectangle(
        img=frame,
        pt1=(x1, y1),
        pt2=(x2, y2),
        color=(0, 255, 0),
        thickness=2
    )

    putText(
        img=frame,
        text=str(student_id),
        org=(x1, y2 + 20),
        fontFace=FONT_HERSHEY_COMPLEX_SMALL,
        fontScale=1,
        color=(255, 255, 255),
        thickness=1,
        lineType=2
    )

    putText(
        img=frame,
        text=str(distance),
        org=(x1, y2 + 40),
        fontFace=FONT_HERSHEY_COMPLEX_SMALL,
        fontScale=1,
        color=(255, 255, 255),
        thickness=1,
        lineType=2
    )


def parse_args(argv):
    parser = ArgumentParser(description='Module to identify faces in an image')
    parser.add_argument('--image', help='Path of image which you want to identify', type=str, required=True)
    args = parser.parse_args(argv)
    return args


if __name__ == '__main__':
    args = parse_args(argv[1:])
    main(args)
    exit(0)
