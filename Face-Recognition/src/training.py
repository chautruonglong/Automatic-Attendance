from math import ceil
from numpy import zeros
from pickle import dump
from sklearn.svm import SVC
from facenet.src.facenet import get_dataset, get_image_paths_and_labels, load_model, load_data
from tensorflow import Graph, Session, get_default_graph

FACENET_MODEL = '../models/premodels/20180402-114759.pb'
OUTPUT_CLASSIFIER = '../models/mymodels/1814.pkl'
INPUT_DATASET = '../dataset/processed/'
BATCH_SIZE = 1
FACE_SIZE = 160
GPU_MEM_FRACTION = 0.25


def main():
    with Graph().as_default():
        with Session().as_default() as sess:

            dataset = get_dataset(INPUT_DATASET)
            img_paths, labels = get_image_paths_and_labels(dataset)

            print(f'Found {len(dataset)} people')
            print(f'Found {len(img_paths)} images')

            load_model(FACENET_MODEL)
            images_placeholder = get_default_graph().get_tensor_by_name("input:0")
            phase_train_placeholder = get_default_graph().get_tensor_by_name("phase_train:0")
            embeddings = get_default_graph().get_tensor_by_name("embeddings:0")
            embedding_size = embeddings.get_shape()[1]

            print('Calculating features for images')
            nrof_images = len(img_paths)
            nrof_batches_per_epoch = int(ceil(nrof_images / BATCH_SIZE))
            embedding_array = zeros((nrof_images, embedding_size))

            print(f'Batches per epoch: {nrof_batches_per_epoch}')

            for index in range(nrof_batches_per_epoch):
                str_index = index * BATCH_SIZE
                end_index = min((index + 1) * BATCH_SIZE, nrof_images)

                print(f'Embedding batch {index}, index images from {str_index} to {end_index}')

                batch_paths = img_paths[str_index:end_index]
                images = load_data(batch_paths, False, False, FACE_SIZE)

                feed_dict = {
                    images_placeholder: images,
                    phase_train_placeholder: False
                }
                embedding_array[str_index:end_index, :] = sess.run(embeddings, feed_dict)

            print('Training image classifier')

            classifier = SVC(kernel='linear', probability=True)
            classifier.fit(embedding_array, labels)

            id_students = [data.name for data in dataset]

            print('Exporting classifier model file')
            with open(OUTPUT_CLASSIFIER, 'wb') as file:
                dump((classifier, id_students), file)


if __name__ == '__main__':
    main()
    exit(0)