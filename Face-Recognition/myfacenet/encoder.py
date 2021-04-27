from cv2 import imread, IMREAD_COLOR
from os.path import sep
from numpy import argmax
from pickle import loads, dumps
from imutils import paths
from facenet.src import facenet
from skimage.transform import resize
from sklearn.preprocessing import LabelEncoder
from myfacenet.classifier import FacenetClassifier
from tensorflow import Session, GPUOptions, ConfigProto, get_default_graph

INPUT_PRETRAIN_MODEL = 'premodels/20180402-114759.pb'
OUTPUT_FACE_CLASSIFIERS = 'mymodels/face_classifiers.pickle'
OUTPUT_FACE_LABELS = 'mymodels/face_labels.pickle'


def save_train(classifier, known_names, known_embeddings):
    label = LabelEncoder()
    known_labels = label.fit_transform(known_names)

    classifier = FacenetClassifier(classifier)
    classifier.fit(known_embeddings, known_labels)

    file = open(OUTPUT_FACE_CLASSIFIERS, 'wb')
    file.write(dumps(classifier))
    file.close()

    file = open(OUTPUT_FACE_LABELS, 'wb')
    file.write(dumps(label))
    file.close()


class FacenetEncoder:
    _face_size = 160
    _face_margin = 0

    def __init__(self, is_training=False):
        gpu = GPUOptions(per_process_gpu_memory_fraction=0.3)
        self._session = Session(config=ConfigProto(gpu_options=gpu, log_device_placement=False))

        with self._session.as_default():
            facenet.load_model(INPUT_PRETRAIN_MODEL)
        if is_training is False:
            classifiers = open(OUTPUT_FACE_CLASSIFIERS, 'rb').read()
            labels = open(OUTPUT_FACE_LABELS, 'rb').read()

            print('Loading my models')

            self._classifier = loads(classifiers)
            self._labels = loads(labels)

    def set_face_size(self, face_size):
        self._face_size = face_size

    def set_face_margin(self, face_margin):
        self._face_margin = face_margin

    def encode(self, img, face_bb):
        x, y, w, h = face_bb
        if self._face_margin == 0:
            x, y, w, h = (
                max(x - int(self._face_margin / 2), 0),
                max(y - int(self._face_margin / 2), 0),
                min(x + w + int(self._face_margin / 2), img.shape[1] - x),
                min(y + h + int(self._face_margin / 2), img.shape[0] - y)
            )
        face = resize(img[y:y + h, x:x + w, :], (self._face_size, self._face_size))

        img_placeholder = get_default_graph().get_tensor_by_name('input:0')
        embeddings = get_default_graph().get_tensor_by_name('embeddings:0')
        phase_train = get_default_graph().get_tensor_by_name('phase_train:0')

        face_prewhiten = facenet.prewhiten(face)
        feed = {
            img_placeholder: [face_prewhiten],
            phase_train: False
        }

        return self._session.run(embeddings, feed_dict=feed)

    def identify(self, img, face_bb):
        vector = self.encode(img, face_bb)
        predict_face = self._classifier.predict(vector)[0]
        id = argmax(predict_face)
        face_id = self._labels.classes_[id]
        confidence = predict_face[id] * 100

        return face_id, confidence

    def train(self, detector, classifier, dataset_path):
        known_embeddings = []
        known_names = []
        img_paths = list(paths.list_images(dataset_path))

        for _, img_path in enumerate(img_paths):
            name = img_path.split(sep)[-2]
            img = imread(img_path, IMREAD_COLOR)
            faces = detector.detect(img)

            print(f'Training image: {img_path}')
            print(f'Found {len(faces)} faces')

            for face_bb in faces:
                vector = self.encode(img, face_bb)
                known_names.append(name)
                known_embeddings.append(vector.flatten())

        print("Training completed")
        print("Exporting pickle file")
        save_train(classifier, known_names, known_embeddings)
