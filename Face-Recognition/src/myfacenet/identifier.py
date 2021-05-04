from pickle import load
from numpy import argmax, arange
from facenet.src.facenet import load_model


class FacenetIdentifier:
    def __init__(self, facenet_model, stored_model):
        if facenet_model is not None:
            load_model(facenet_model)

        with open(stored_model, 'rb') as file:
            self._classifier, self._id_students = load(file)
            print(f'Successfully loaded classifier model: {file.name}')

    def reload_stored_model(self, stored_model):
        with open(stored_model, 'rb') as file:
            self._classifier, self._id_students = load(file)
            print(f'Successfully reloaded classifier model: {file.name}')

    def identify(self, face_embedding):
        predictions = self._classifier.predict_proba(face_embedding)
        best_student_indices = argmax(predictions, axis=1)
        confidence = predictions[arange(len(best_student_indices)), best_student_indices][0]
        id_student = self._id_students[best_student_indices[0]]

        return id_student, confidence
