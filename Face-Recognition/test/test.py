from src.myfacenet.detector import MTCNNDetector
from src.myfacenet.encoder import FacenetEncoder
from src.myfacenet.identifier import FacenetIdentifier
import cv2
from tensorflow import Graph, Session, ConfigProto, GPUOptions

with Graph().as_default():
    gpu_options = GPUOptions(per_process_gpu_memory_fraction=0.25)
    sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

    with sess.as_default():

        detector = MTCNNDetector(sess)
        encoder = FacenetEncoder('/home/chautruonglong/Desktop/Face-Recognition/models/premodels/20180402-114759.pb', 160)
        identifier = FacenetIdentifier(None, '/home/chautruonglong/Desktop/Face-Recognition/models/mymodels/1814.pkl')

        img = cv2.imread('/home/chautruonglong/Pictures/huy2.png')
        faces = detector.detect(img)

        cv2.namedWindow('Face Window', cv2.WINDOW_NORMAL)

        for x1, y1, x2, y2 in faces:
            face_embedding = encoder.encode_face(sess, img[y1:y2, x1:x2])
            id_student, confidence = identifier.identify(face_embedding)
            print(id_student, confidence)
            cv2.rectangle(img, (x1, y1), (x2, y2), (0, 255, 0), 2)

        cv2.imshow('Face Window', img)
        cv2.waitKey(0)
        cv2.destroyAllWindows()
        exit(0)
