from argparse import ArgumentParser
from os.path import isfile
from sys import argv

from cv2 import imread, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows


def parse_args(argv):
    parser = ArgumentParser(description='Module to identify faces in an image')
    parser.add_argument('--image', help='Path of image which you want to identify', type=str, required=True)
    args = parser.parse_args(argv)
    return args


def main(args):
    if isfile(args.image):
        namedWindow('webcam', WINDOW_NORMAL)

        img = imread(args.image)

        imshow('webcam', img)

        if waitKey(1) & 0xFF == ord('q'):
            destroyAllWindows()

    else:
        print('File does not exits')


if __name__ == '__main__':
    args = parse_args(argv[1:])
    main(args)
    exit(0)
