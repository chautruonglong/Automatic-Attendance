import os
import cv2

INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/raw/mem/minh_vu/'

HAARCASADE_PATH = '/home/chautruonglong/Desktop/Face-Recognition/models/premodels/haarcascade/haarcascade_frontalface_default.xml'

detector = cv2.CascadeClassifier(HAARCASADE_PATH)

files = os.listdir(INPUT_PATH)

if not os.path.exists(INPUT_PATH + 'out'):
    os.makedirs(INPUT_PATH + 'out')

num = 69

for file in files:
    name, ext = file.split('.')

    if ext == 'jpg':
        img = cv2.imread(INPUT_PATH + file)
        faces = detector.detectMultiScale(img, 1.1, 4)
  
        for x, y, w, h in faces:
            new_img = img[y:y + h, x:x + w]
            file = INPUT_PATH + 'out/' + str(num) + '.png'
            cv2.imwrite(file, new_img)
            print('Saving ' + file)
            num += 1