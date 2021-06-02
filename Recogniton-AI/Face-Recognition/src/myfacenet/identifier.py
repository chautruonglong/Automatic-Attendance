from pickle import load
from numpy import argmax, arange
from facenet.src.facenet import load_model


class FacenetIdentifier:
    def __init__(self, facenet_model, classifier_model):
        if facenet_model is not None:
            load_model(facenet_model)

        with open(classifier_model, 'rb') as file:
            self._classifier, self._student_ids = load(file)
            print(f'Successfully loaded classifier model: {file.name}')

    def reload_classifier_model(self, classifier_model):
        with open(classifier_model, 'rb') as file:
            self._classifier, self._student_ids = load(file)
            print(f'Successfully reloaded classifier model: {file.name}')

    def identify(self, face_embedding):
        predictions = self._classifier.predict_proba(face_embedding)
        best_student_indices = argmax(predictions, axis=1)
        confidence = predictions[arange(len(best_student_indices)), best_student_indices][0]
        student_id = self._student_ids[best_student_indices[0]]

        return student_id, confidence
