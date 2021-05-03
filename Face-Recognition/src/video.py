from argparse import ArgumentParser
from os.path import isfile
from sys import argv

from cv2 import VideoCapture, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows


def parse_args(argv):
    parser = ArgumentParser(description='Module to identify faces in a video')
    parser.add_argument('--video', help='Path of video which you want to identify', type=str, required=True)
    args = parser.parse_args(argv)
    return args


def main(args):
    if isfile(args.video):
        capture = VideoCapture()
        namedWindow('webcam', WINDOW_NORMAL)

        while capture.isOpened():
            ret, frame = capture.read()
            imshow('webcam', frame)

            if waitKey(1) & 0xFF == ord('q'):
                break

        capture.release()
        destroyAllWindows()

    else:
        print('File does not exits')


if __name__ == '__main__':
    args = parse_args(argv[1:])
    main(args)
    exit(0)
