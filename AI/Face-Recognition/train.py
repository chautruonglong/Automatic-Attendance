from os import path, makedirs, walk
from myfacenet.detector import FaceDetector, DetectorModels
from myfacenet.encoder import FacenetEncoder
from myfacenet.classifier import ClassifierModels


INPUT_DIR_DATASETS = 'D:/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/faces'


def create_folder(folder):
    if not path.exists(folder):
        makedirs(folder)


if __name__ == '__main__':
    detector = FaceDetector(detector_model=DetectorModels.HAARCASCADE)

    encoder = FacenetEncoder(is_training=True)

    encoder.train(
        detector=detector,
        classifier=ClassifierModels.RBF,
        dataset_path=INPUT_DIR_DATASETS
    )
