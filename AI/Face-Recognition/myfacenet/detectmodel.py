from cv2 import cvtColor, equalizeHist, COLOR_BGR2GRAY
from cv2 import CascadeClassifier
from dlib import get_frontal_face_detector
from facenet.src.align.detect_face import detect_face, create_mtcnn
from tensorflow import Graph
from tensorflow.compat.v1 import GPUOptions, ConfigProto, Session


class HaarcascadeDetector:
    _HAARCASCADE_MODEL = '../premodels/haarcascade_frontalface_default.xml'

    def __init__(self, face_size):
        self._face_size = face_size
        self._detector = CascadeClassifier(self._HAARCASCADE_MODEL)

    def detect(self, img):
        img = cvtColor(img, COLOR_BGR2GRAY)
        img = equalizeHist(img)
        faces = self._detector.detectMultiScale(img, 1.1, 5, minSize=(self._face_size, self._face_size))
        return faces


class FacenetDetector:
    _THRESHOLD = [0.6, 0.7, 0.7]
    _FACTOR = 0.709

    def __init__(self, face_size):
        self._face_size = face_size

        with Graph().as_default():
            gpu = GPUOptions(per_process_gpu_memory_fraction=0.3)
            session = Session(config=ConfigProto(gpu_options=gpu, log_device_placement=False))
            with session.as_default():
                self._pnet, self._rnet, self._onet = create_mtcnn(session, model_path=None)

    def detect(self, img):
        faces, _ = detect_face(
            img,
            self._face_size,
            self._pnet,
            self._rnet,
            self._onet,
            self._THRESHOLD,
            self._FACTOR
        )

        faces_updated = []
        for face in faces:
            face = face.astype('int')
            rect = (
                max(face[0], 0),
                max(face[1], 0),
                min(face[2], img.shape[1] - max(face[0], 0)),
                min(face[3], img.shape[0] - max(face[1], 0))
            )
            faces_updated.append(rect)

        return faces_updated


class DlibHOGDetector:
    def __init__(self, face_size):
        self._face_size = face_size
        self._detector = get_frontal_face_detector()

    def detect(self, img):
        img = cvtColor(img, COLOR_BGR2GRAY)
        img = equalizeHist(img)
        faces = self._detector(img, 0)
        return faces
