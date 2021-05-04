from cv2 import CascadeClassifier, cvtColor, COLOR_BGR2GRAY
from facenet.src.align.detect_face import create_mtcnn, detect_face


class HaarcascadeDetector:
    def __init__(self, model_path, min_size):
        self._detector = CascadeClassifier(model_path)
        self._min_size = min_size

    def detect(self, img):
        img = cvtColor(img, COLOR_BGR2GRAY)
        faces = self._detector.detectMultiScale(
            image=img,
            scaleFactor=1.2,
            minNeighbors=5,
            minSize=(self._min_size, self._min_size)
        )

        updated_faces = [
            (x, y, x + w, y + h) for x, y, w, h in faces
        ]

        return updated_faces


class MTCNNDetector:
    _GPU_MEM_FRACTION = 0.3
    _THRESHOLDS = [0.6, 0.7, 0.7]
    _FACTOR = 0.709

    def __init__(self, sess, model_path, min_size):
        self._pnet, self._rnet, self._onet = create_mtcnn(sess, model_path)
        self._min_size = min_size

    def detect(self, img):
        faces, _ = detect_face(
            img,
            self._min_size,
            self._pnet, self._rnet, self._onet,
            self._THRESHOLDS,
            self._FACTOR
        )

        return faces
