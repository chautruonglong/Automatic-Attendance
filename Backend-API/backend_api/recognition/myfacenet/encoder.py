from cv2 import resize, INTER_LANCZOS4, GaussianBlur
from facenet.src.facenet import load_model, prewhiten
from tensorflow import get_default_graph


class FacenetEncoder:
    def __init__(self, facenet_model, face_size, sess):
        if facenet_model is not None:
            load_model(facenet_model)

        self._face_size = face_size
        self._sess = sess
        self._images_placeholder = get_default_graph().get_tensor_by_name("input:0")
        self._phase_train_placeholder = get_default_graph().get_tensor_by_name("phase_train:0")
        self._embeddings = get_default_graph().get_tensor_by_name("embeddings:0")
        self._num_features = int(self._embeddings.get_shape()[1])

    def get_num_features(self):
        return self._num_features

    def encode_training(self, faces):
        feed_dict = {
            self._images_placeholder: faces,
            self._phase_train_placeholder: False
        }

        face_embeddings = self._sess.run(self._embeddings, feed_dict)
        return face_embeddings

    def encode_face(self, face):
        face = GaussianBlur(face, (3, 3), 0)
        w, h, c = face.shape

        if w != self._face_size or h != self._face_size:
            face = resize(face, (self._face_size, self._face_size), interpolation=INTER_LANCZOS4)

        face = prewhiten(face)
        face = face.reshape(-1, self._face_size, self._face_size, 3)

        feed_dict = {
            self._images_placeholder: face,
            self._phase_train_placeholder: False
        }

        face_embedding = self._sess.run(self._embeddings, feed_dict)
        return face_embedding
