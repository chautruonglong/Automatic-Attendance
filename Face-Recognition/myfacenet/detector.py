from enum import Enum
from myfacenet.detectmodel import HaarcascadeDetector
from myfacenet.detectmodel import MTCNNDetector
from myfacenet.detectmodel import FacenetDetector
from myfacenet.detectmodel import DlibHOGDetector


class DetectorModels(Enum):
    HAARCASCADE = 0
    MTCNN = 1
    FACENET = 2
    DLIB_HOG = 3


class FaceDetector:
    def __init__(self, detector_model, face_size=10):
        self._detector = None
        if detector_model is DetectorModels.HAARCASCADE:
            self._detector = HaarcascadeDetector(face_size)
        elif detector_model is DetectorModels.MTCNN:
            self._detector = MTCNNDetector(face_size)
        elif detector_model is DetectorModels.FACENET:
            self._detector = FacenetDetector(face_size)
        elif detector_model is DetectorModels.DLIB_HOG:
            self._detector = DlibHOGDetector(face_size)
        else:
            raise Exception('Not using detector model')

        print(f'Using detector model: {DetectorModels(detector_model)}')

    def detect(self, img):
        return self._detector.detect(img)
