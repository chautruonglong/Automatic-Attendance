from tensorflow import get_default_graph
from facenet.src.facenet import load_model, prewhiten
from cv2 import resize, INTER_LANCZOS4


class FacenetEncoder:
    def __init__(self, facenet_model, face_size):
        if facenet_model is not None:
            load_model(facenet_model)

        self._face_size = face_size

        self._images_placeholder = get_default_graph().get_tensor_by_name("input:0")
        self._phase_train_placeholder = get_default_graph().get_tensor_by_name("phase_train:0")
        self._embeddings = get_default_graph().get_tensor_by_name("embeddings:0")
        self._num_features = self._embeddings.get_shape()[1]

    def get_num_features(self):
        return self._num_features

    def encode_training(self, sess, faces):
        feed_dict = {
            self._images_placeholder: faces,
            self._phase_train_placeholder: False
        }

        face_embeddings = sess.run(self._embeddings, feed_dict)
        return face_embeddings

    def encode_face(self, sess, face):
        scaled = resize(face, (self._face_size, self._face_size), interpolation=INTER_LANCZOS4)
        scaled = prewhiten(scaled)
        scaled_reshape = scaled.reshape(-1, self._face_size, self._face_size, 3)

        feed_dict = {
            self._images_placeholder: scaled_reshape,
            self._phase_train_placeholder: False
        }

        face_embeddings = sess.run(self._embeddings, feed_dict)
        return face_embeddings
