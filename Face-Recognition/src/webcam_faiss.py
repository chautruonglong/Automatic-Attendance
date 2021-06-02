from src.myfacenet.detector import MTCNNDetector
from src.myfacenet.encoder import FacenetEncoder
from src.myfacenet.indentifier_faiss import FaissIdentifier
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from cv2 import rectangle, putText, FONT_HERSHEY_COMPLEX_SMALL, flip
from cv2 import VideoCapture, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows

FACENET_MODEL = '../models/premodels/512/20180402-114759.pb'
INDEXING_MODEL = '../models/mymodels/1814_140_1.index'
MTCNN_MODEL = '../models/premodels/align'
HAARCASCADE_MODEL = '../models/premodels/haarcascade/haarcascade_frontalface_default.xml'
THRESHOLD = 0.4
GPU_MEM_FRACTION = 0.3
FACE_SIZE = 140
MIN_SIZE = 20


def main():
    with Graph().as_default():
        gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
        sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

        with sess.as_default():
            detector = MTCNNDetector(sess, MTCNN_MODEL, MIN_SIZE)
            encoder = FacenetEncoder(sess, FACENET_MODEL, FACE_SIZE)
            identifier = FaissIdentifier(None, INDEXING_MODEL)

            capture = VideoCapture(0)
            namedWindow('webcam', WINDOW_NORMAL)

            while capture.isOpened():
                try:
                    ret, frame = capture.read()
                    frame = flip(frame, 1)
                    faces = detector.detect(frame)

                    for face in faces:
                        x1 = int(face[0])
                        y1 = int(face[1])
                        x2 = int(face[2])
                        y2 = int(face[3])

                        if x1 > 0 and y1 > 0 and x2 > 0 and y2 > 0:
                            face_embedding = encoder.encode_face(frame[y1:y2, x1:x2])
                            distance, student_id = identifier.identify(face_embedding)
                            distance = round(distance, 2)

                            if distance < THRESHOLD:
                                put_face_label(frame, x1, y1, x2, y2, student_id, distance)
                            else:
                                put_face_label(frame, x1, y1, x2, y2, 'Unknown', 'Nan')

                            print(f'Id: {student_id}, confidence: {distance}')

                    imshow('webcam', frame)

                    if waitKey(1) & 0xFF == ord('q'):
                        break

                except Exception as error:
                    print(f'Frame error: {error}')

            capture.release()
            destroyAllWindows()


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


if __name__ == '__main__':
    main()
    exit(0)
