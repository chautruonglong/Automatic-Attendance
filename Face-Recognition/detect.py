from os.path import isfile
from argparse import ArgumentParser
from myfacenet.detector import FaceDetector
from myfacenet.detector import DetectorModels
from cv2 import rectangle, imshow, imread
from cv2 import waitKey, destroyAllWindows, namedWindow, WINDOW_NORMAL

if __name__ == '__main__':
    parser = ArgumentParser(description='Module to detect faces in an image')
    parser.add_argument('--image', type=str, required=True, help='Path of an image')
    parser.add_argument('--min_size', type=int, required=False, help='Min face size in an image, default 10 pixel', default=10)

    args = parser.parse_args()

    if isfile(args.image) is True:
        img = imread(args.image)

        detector = FaceDetector(detector_model=DetectorModels.HAARCASCADE, face_size=args.min_size)
        faces = detector.detect(img=img)

        namedWindow('Face Window', WINDOW_NORMAL)

        for x, y, w, h in faces:
            rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)

        imshow('Face Window', img)

        waitKey(0)
        destroyAllWindows()
        exit(0)

    else:
        print('File does not exist')
