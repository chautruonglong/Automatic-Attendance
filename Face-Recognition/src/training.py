from math import ceil
from numpy import zeros
from pickle import dump
from sklearn.svm import SVC
from sklearn.neighbors import KNeighborsClassifier
from facenet.src.facenet import get_dataset, get_image_paths_and_labels, load_data
from src.myfacenet.encoder import FacenetEncoder
from tensorflow import Graph, Session, GPUOptions, ConfigProto

FACENET_MODEL = '../models/premodels/20180402-114759.pb'
OUTPUT_CLASSIFIER = '../models/mymodels/1814.pkl'
INPUT_DATASET = '../dataset/processed/'
BATCH_SIZE = 2000
FACE_SIZE = 160
GPU_MEM_FRACTION = 0.25


def main():
    with Graph().as_default():
        gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
        sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

        with sess.as_default():

            dataset = get_dataset(INPUT_DATASET)
            img_paths, labels = get_image_paths_and_labels(dataset)

            print(f'Found {len(dataset)} people')
            print(f'Found {len(img_paths)} images')

            encoder = FacenetEncoder(FACENET_MODEL, FACE_SIZE)
            embedding_size = encoder.get_num_features()

            print('Calculating features for images')

            nrof_images = len(img_paths)
            nrof_batches_per_epoch = int(ceil(nrof_images / BATCH_SIZE))
            embedding_array = zeros((nrof_images, embedding_size))

            print(f'Batches per epoch: {nrof_batches_per_epoch}')

            for index in range(nrof_batches_per_epoch):
                str_index = index * BATCH_SIZE
                end_index = min((index + 1) * BATCH_SIZE, nrof_images)

                print(f'Embedding batch {index}/{nrof_batches_per_epoch - 1}, index images from {str_index} to {end_index}')

                batch_paths = img_paths[str_index:end_index]
                faces = load_data(batch_paths, False, False, FACE_SIZE)

                embedding_array[str_index:end_index, :] = encoder.encode_training(sess, faces)

            print('Training image classifier')

            id_students = [data.name for data in dataset]

            classifier = SVC(kernel='rbf', probability=True)
            # classifier = KNeighborsClassifier(n_neighbors=len(id_students), metric='euclidean')
            classifier.fit(embedding_array, labels)

            print('Exporting classifier model file')

            with open(OUTPUT_CLASSIFIER, 'wb') as file:
                dump((classifier, id_students), file)


if __name__ == '__main__':
    main()
    exit(0)
