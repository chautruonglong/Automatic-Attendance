from cv2 import CascadeClassifier, cvtColor, COLOR_BGR2GRAY
from facenet.src.align.detect_face import create_mtcnn, detect_face
from tensorflow import Graph, Session, ConfigProto, GPUOptions


class HaarcascadeDetector:
    _HAARCASCADE_MODEL = '../models/premodels/haarcascade_frontalface_default.xml'
    _MIN_SIZE = 20

    def __init__(self):
        self._detector = CascadeClassifier(self._HAARCASCADE_MODEL)

    def detect(self, img):
        img = cvtColor(img, COLOR_BGR2GRAY)
        faces = self._detector.detectMultiScale(
            image=img,
            scaleFactor=1.2,
            minNeighbors=5,
            minSize=(self._MIN_SIZE, self._MIN_SIZE)
        )

        updated_faces = [
            (x, y, x + w, y + h) for x, y, w, h in faces
        ]

        return updated_faces


class MTCNNDetector:
    _GPU_MEM_FRACTION = 0.3
    _MIN_SIZE = 20
    _THRESHOLDS = [0.6, 0.7, 0.7]
    _FACTOR = 0.709

    def __init__(self):
        with Graph().as_default():
            gpu_options = GPUOptions(per_process_gpu_memory_fraction=self._GPU_MEM_FRACTION)
            sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

            with sess.as_default():
                self._pnet, self._rnet, self._onet = create_mtcnn(sess, None)

    def detect(self, img):
        faces, _ = detect_face(
            img,
            self._MIN_SIZE,
            self._pnet, self._rnet, self._onet,
            self._THRESHOLDS,
            self._FACTOR
        )

        updated_faces = [
            (int(x1), int(y1), int(x2), int(y2)) for x1, y1, x2, y2, c in faces
        ]

        return updated_faces
