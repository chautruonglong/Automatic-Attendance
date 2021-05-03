from src.myfacenet.detector import MTCNNDetector, HaarcascadeDetector
import cv2

detector = HaarcascadeDetector()

img = cv2.imread('/home/chautruonglong/Pictures/huy2.png')
faces = detector.detect(img)

cv2.namedWindow('Face Window', cv2.WINDOW_NORMAL)

for x1, y1, x2, y2 in faces:
    cv2.rectangle(img, (x1, y1), (x2, y2), (0, 255, 0), 2)

cv2.imshow('Face Window', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
exit(0)