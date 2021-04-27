from os.path import isfile
from argparse import ArgumentParser
from myfacenet.detector import FaceDetector
from myfacenet.detector import DetectorModels
from myfacenet.encoder import FacenetEncoder
from cv2 import imread, imshow, putText, rectangle, imwrite
from cv2 import FONT_HERSHEY_SIMPLEX, LINE_AA, WINDOW_NORMAL
from cv2 import namedWindow, waitKey, destroyAllWindows

THRESHOLD = 50


def set_label_for_bb(img, face, id, confidence):
    x, y, w, h = face
    rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)

    if id is None:
        id, confidence = 'Unknown', 0.0

    putText(
        img=img,
        text=f'{id} - {round(confidence, 2)}%',
        org=(x + 5, y - 5),
        fontFace=FONT_HERSHEY_SIMPLEX,
        fontScale=1,
        color=(60, 20, 220),
        thickness=1,
        lineType=LINE_AA
    )


if __name__ == '__main__':
    parser = ArgumentParser('Module to identify faces in an images')
    parser.add_argument('--image', type=str, required=True, help='Path of an image')
    parser.add_argument('--min_size', type=int, required=False, help='Min face size in an image, default 10 pixel', default=10)

    args = parser.parse_args()

    if isfile(args.image) is True:
        encoder = FacenetEncoder(is_training=False)
        detector = FaceDetector(detector_model=DetectorModels.HAARCASCADE, face_size=args.min_size)

        img = imread(args.image)
        faces = detector.detect(img=img)

        namedWindow('Face Window', WINDOW_NORMAL)

        for face in faces:
            id, confidence = encoder.identify(img, face)

            if confidence > THRESHOLD:
                set_label_for_bb(img, face, id, confidence)
            else:
                set_label_for_bb(img, face, None, None)

        imshow('Face Window', img)
        imwrite('long.jpg', img)
        waitKey(0)
        destroyAllWindows()
        exit(0)

    else:
        print('File does not exist')
