import cv2
import numpy as np

INPUT_IMG = '../output/face_0.jpg'
OUTPUT_IMG = 'face_blur.jpg'
IMG_SIZE = 140

if __name__ == '__main__':
    gaussian = (1.0 / 16) * np.array(
        [[1, 2, 1],
         [2, 4, 2],
         [1, 2, 1]])

    img = cv2.imread(INPUT_IMG)
    img = cv2.resize(img, (IMG_SIZE, IMG_SIZE))
    img = cv2.GaussianBlur(img, (9, 9), 0)
    cv2.imwrite(OUTPUT_IMG, img)
