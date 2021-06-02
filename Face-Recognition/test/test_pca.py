from src.myfacenet.encoder import FacenetEncoder
from sklearn.decomposition import PCA
from sklearn.svm import SVC
from cv2 import imread
from tensorflow import Session, GPUOptions, ConfigProto, Graph
from numpy.random import randint

FACENET_MODEL = '../models/premodels/512/20180402-114759.pb'
FACE_SIZE = 140
GPU_MEM_FRACTION = 0.25


if __name__ == '__main__':
    with Graph().as_default():
        gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
        sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

        with sess.as_default():
            encoder = FacenetEncoder(FACENET_MODEL, 140)
            pca = PCA(n_components=128)
            svc = SVC(probability=True)
            img = imread('face.png')

            inputs = randint(1, 100, size=(1000, 512))
            print(len(inputs))
            print(len(inputs[0]))

            embeddings = pca.fit_transform(inputs)
            svc.fit(embeddings, range(1000))

            print(len(embeddings))
            print(len(embeddings[0]))
