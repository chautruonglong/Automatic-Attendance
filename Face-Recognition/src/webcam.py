from cv2 import VideoCapture, imshow, namedWindow, WINDOW_NORMAL
from cv2 import waitKey, destroyAllWindows


def main():
    # Object for capture webcam
    capture = VideoCapture(0)
    # Set full size for window
    namedWindow('webcam', WINDOW_NORMAL)

    while capture.isOpened():
        # Show frame
        ret, frame = capture.read()
        imshow('webcam', frame)

        # Turn off app when press q button
        if waitKey(1) & 0xFF == ord('q'):
            break

    # Release webcam and destroy app
    capture.release()
    destroyAllWindows()


if __name__ == '__main__':
    # Start app
    main()
    exit(0)
