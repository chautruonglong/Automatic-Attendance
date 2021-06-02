from src.enhancer.enhancer import Enhancer
import cv2

INPUT_MODEL = '../models/premodels/esrgan/RRDB_ESRGAN_x4.pth'
enhancer = Enhancer(INPUT_MODEL)

img = cv2.imread('face_blur.jpg', cv2.IMREAD_COLOR)

new_img = enhancer.enhance(img)
# new_img = cv2.resize(new_img, (160, 160))

cv2.imwrite('enhance.png', new_img)
