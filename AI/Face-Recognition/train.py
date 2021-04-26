from os import path, makedirs, walk
from myfacenet.detector import FaceDetector
from myfacenet.encoder import FacenetEncoder


INPUT_DIR_DATASETS = 'D:/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/faces'


def create_folder(folder):
    if not path.exists(folder):
        makedirs(folder)


if __name__ == '__main__':
    pass
