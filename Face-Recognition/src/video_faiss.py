from argparse import ArgumentParser
from os.path import isfile
from sys import argv
from myfacenet.detector import MTCNNDetector
from myfacenet.encoder import FacenetEncoder
from myfacenet.indentifier_faiss import FaissIdentifier
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from cv2 import rectangle, putText, FONT_HERSHEY_COMPLEX_SMALL
from cv2 import VideoCapture, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows

FACENET_MODEL = 'models/premodels/20180402-114759.pb'
INDEXING_MODEL = 'models/mymodels/1814_140_1.index'
MTCNN_MODEL = 'models/premodels/align'
HAARCASCADE_MODEL = 'models/premodels/haarcascade_frontalface_default.xml'
THRESHOLD = 0.5
GPU_MEM_FRACTION = 0.3
FACE_SIZE = 140
MIN_SIZE = 20


def main(args):
    if isfile(args.video):
        with Graph().as_default():
            gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
            sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

            with sess.as_default():
                detector = MTCNNDetector(sess, MTCNN_MODEL, MIN_SIZE)
                encoder = FacenetEncoder(FACENET_MODEL, FACE_SIZE)
                identifier = FaissIdentifier(None, INDEXING_MODEL)

                capture = VideoCapture(args.video)
                namedWindow('video', WINDOW_NORMAL)

                while capture.isOpened():
                    try:
                        ret, frame = capture.read()
                        faces = detector.detect(frame)

                        for face in faces:
                            x1 = int(face[0])
                            y1 = int(face[1])
                            x2 = int(face[2])
                            y2 = int(face[3])

                            face_embedding = encoder.encode_face(sess, frame[y1:y2, x1:x2])
                            distance, student_id = identifier.identify(face_embedding)
                            distance = round(distance, 2)

                            if distance < THRESHOLD:
                                put_face_label(frame, x1, y1, x2, y2, student_id, distance)
                            else:
                                put_face_label(frame, x1, y1, x2, y2, 'Unknown', '')

                            print(f'Id: {student_id}, confidence: {distance}')

                        imshow('video', frame)

                        if waitKey(1) & 0xFF == ord('q'):
                            break

                    except Exception as error:
                        print(f'Frame error: {error}')

                capture.release()
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
    parser = ArgumentParser(description='Module to identify faces in a video')
    parser.add_argument('--video', help='Path of video which you want to identify', type=str, required=True)
    args = parser.parse_args(argv)
    return args


if __name__ == '__main__':
    args = parse_args(argv[1:])
    main(args)
    exit(0)
