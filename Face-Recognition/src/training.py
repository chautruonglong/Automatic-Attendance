from math import ceil
from numpy import zeros, array, float32
from pickle import dump
from sklearn.svm import SVC
from sklearn.neighbors import KNeighborsClassifier
from facenet.src.facenet import get_dataset, get_image_paths_and_labels, load_data
from src.myfacenet.encoder import FacenetEncoder
from tensorflow import Graph, Session, GPUOptions, ConfigProto
from faiss import IndexFlatL2, IndexIDMap, write_index

OUTPUT_SVM_CLASSIFIER = '../models/mymodels/1814_140s_512d_svm_big.pkl'
OUTPUT_KNN_CLASSIFIER = '../models/mymodels/1814_140s_512d_knn_big.pkl'
OUTPUT_FAISS_INDEXING = '../models/mymodels/1814_140s_512d_fai_big.ind'

FACENET_MODEL = '../models/premodels/512/20180402-114759.pb'
INPUT_DATASET = '../dataset/processed/'
BATCH_SIZE = 200
FACE_SIZE = 140
GPU_MEM_FRACTION = 0.4


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

            print(f'Embedding size: {embedding_size}')
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

            student_ids = [data.name for data in dataset]

            print(f'Exporting svm classifier model: {OUTPUT_SVM_CLASSIFIER}')

            svm_classifier = SVC(kernel='rbf', probability=True)
            svm_classifier.fit(embedding_array, labels)
            with open(OUTPUT_SVM_CLASSIFIER, 'wb') as file:
                dump((svm_classifier, student_ids), file)

            print(f'Exporting knn classifier model: {OUTPUT_KNN_CLASSIFIER}')

            knn_classifier = KNeighborsClassifier(n_neighbors=20, metric='cosine')
            knn_classifier.fit(embedding_array, labels)
            with open(OUTPUT_KNN_CLASSIFIER, 'wb') as file:
                dump((knn_classifier, student_ids), file)

            print(f'Exporting faiss indexing file: {OUTPUT_FAISS_INDEXING}')

            labels = array(labels)
            student_ids = array(student_ids, dtype=int)
            for i in range(len(student_ids)):
                labels[labels == i] = student_ids[i]

            embedding_index = IndexFlatL2(embedding_size)
            embedding_index = IndexIDMap(embedding_index)
            embedding_index.add_with_ids(array(embedding_array, dtype=float32), array(labels))
            write_index(embedding_index, OUTPUT_FAISS_INDEXING)


if __name__ == '__main__':
    main()
    exit(0)
