from numpy import float32, array
from facenet.src.facenet import load_model
from faiss import read_index


class FaissIdentifier:
    def __init__(self, facenet_model, indexing_model):
        if facenet_model is not None:
            load_model(facenet_model)

        with open(indexing_model, 'rb') as file:
            self._embedding_index = read_index(indexing_model)
            print(f'Successfully loaded classifier model: {file.name}')

    def reload_classifier_model(self, indexing_model):
        with open(indexing_model, 'rb') as file:
            self._embedding_index = read_index(indexing_model)
            print(f'Successfully reloaded classifier model: {file.name}')

    def identify(self, face_embedding):
        distances, student_ids = self._embedding_index.search(array(face_embedding, dtype=float32), k=1)
        return distances[0][0], student_ids[0][0]
