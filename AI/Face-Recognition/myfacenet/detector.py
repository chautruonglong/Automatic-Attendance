from enum import Enum
from myfacenet.detectmodel import HaarcascadeDetector
from myfacenet.detectmodel import FacenetDetector
from myfacenet.detectmodel import DlibHOGDetector


class DetectorModels(Enum):
    HAARCASCADE = 0
    FACENET = 1
    DLIB_CNN = 2


class FaceDetector:
    def __init__(self, detector_model, is_optimize=False, face_size=20):
        if is_optimize is True:
            face_size = max(180, face_size)

        self._detector = None
        if detector_model is DetectorModels.HAARCASCADE:
            self._detector = HaarcascadeDetector(face_size)
        elif detector_model is DetectorModels.FACENET:
            self._detector = FacenetDetector(face_size)
        elif detector_model is DetectorModels.DLIB_CNN:
            self._detector = DlibHOGDetector(face_size)
        else:
            raise Exception('Not using detector model')

        print(f'Using detector model: {DetectorModels(detector_model)}')

    def detect(self, img):
        return self._detector.detect(img)
