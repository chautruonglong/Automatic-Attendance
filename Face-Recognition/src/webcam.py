from src.myfacenet.detector import MTCNNDetector, HaarcascadeDetector
from src.myfacenet.encoder import FacenetEncoder
from src.myfacenet.identifier import FacenetIdentifier
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from cv2 import rectangle, putText, FONT_HERSHEY_COMPLEX_SMALL, flip
from cv2 import VideoCapture, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows
# from sklearn.decomposition import PCA
# from pickle import load

CLASSIFIER_MODEL = '../models/mymodels/data_jpg/1814_140s_128d_svm_big.pkl'
# PCA_MODEL = '../models/mymodels/data_pca/1814_140s_64d_pca.pkl'
FACENET_MODEL = '../models/premodels/128/20170512-110547.pb'
MTCNN_MODEL = '../models/premodels/align'
HAARCASCADE_MODEL = '../models/premodels/haarcascade/haarcascade_frontalface_default.xml'
THRESHOLD = 30
GPU_MEM_FRACTION = 0.4
FACE_SIZE = 140
MIN_SIZE = 100


def main():
    with Graph().as_default():
        gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
        sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

        with sess.as_default():
            detector = MTCNNDetector(sess, MTCNN_MODEL, MIN_SIZE)
            encoder = FacenetEncoder(sess, FACENET_MODEL, FACE_SIZE)
            identifier = FacenetIdentifier(None, CLASSIFIER_MODEL)

            # with open(PCA_MODEL, 'rb') as file:
            #     pca = load(file)
            #     print(f'Successfully loaded pca model: {PCA_MODEL}')

            capture = VideoCapture(1)
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
                            # face_embedding = pca.transform(face_embedding)
                            id_student, confidence = identifier.identify(face_embedding)

                            confidence *= 100
                            confidence = round(confidence, 1)

                            if confidence > THRESHOLD:
                                put_face_label(frame, x1, y1, x2, y2, id_student, confidence)
                            else:
                                put_face_label(frame, x1, y1, x2, y2, 'Unknown', '')

                    imshow('webcam', frame)

                    if waitKey(1) & 0xFF == ord('q'):
                        break

                except Exception as error:
                    print(f'Frame error: {error}')

            capture.release()
            destroyAllWindows()


def put_face_label(frame, x1, y1, x2, y2, id_student, confidence):
    rectangle(
        img=frame,
        pt1=(x1, y1),
        pt2=(x2, y2),
        color=(0, 255, 0),
        thickness=2
    )

    putText(
        img=frame,
        text=id_student,
        org=(x1, y2 + 20),
        fontFace=FONT_HERSHEY_COMPLEX_SMALL,
        fontScale=1,
        color=(255, 255, 255),
        thickness=1,
        lineType=2
    )

    putText(
        img=frame,
        text=str(confidence),
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
