import cv2

cv2.namedWindow('test camera', cv2.WINDOW_NORMAL)
camera = cv2.VideoCapture(1)

while True:
    ret, frame = camera.read()
    frame = cv2.flip(frame, 1)
    cv2.imshow('test camera', frame)
    if cv2.waitKey(1) == ord('q'):
        break

camera.release()
cv2.destroyAllWindows()
