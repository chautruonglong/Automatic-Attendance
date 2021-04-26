from enum import Enum
from facenet.src.classifier import SVC


class ClassifierModels(Enum):
    LINEAR = 0
    RBF = 1


class FacenetClassifier:
    def __init__(self, classifier_model):
        self._classifier = None

        if classifier_model is ClassifierModels.LINEAR:
            self._classifier = SVC(C=1.0, kernel='linear', probability=True)
        elif classifier_model is ClassifierModels.RBF:
            self._classifier = SVC(C=1.0, kernel='rbf', probability=True)
        else:
            raise Exception('Not using classifier model')

        print(f'Using classifier model: {ClassifierModels(classifier_model)}')

    def fit(self, embeddings, labels):
        self._classifier.fit(embeddings, labels)

    def predict(self, vector):
        return self._classifier.predict(vector)
