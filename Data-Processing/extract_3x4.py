import os
import cv2

INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Dataset-3x4/Cong-nghe-thong-tin/K18/'
OUTPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'

HAARCASADE_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Automatic-Attendance/Face-Recognition/premodels/haarcascade_frontalface_default.xml'

MIN_ID = 102180153
MAX_ID = 102180196

detector = cv2.CascadeClassifier(HAARCASADE_PATH)

for id in range(MIN_ID, MAX_ID):
    
    path = OUTPUT_PATH + str(id)
    if not os.path.exists(path):
        os.makedirs(path)
    
    num = len(os.listdir(path))
    
    img = cv2.imread(INPUT_PATH + str(id) + '.jpg')
    faces = detector.detectMultiScale(img, 1.3, 5)
    
    for x, y, w, h in faces:
        new_img = img[y:y + h, x:x + w]
        file = path + '/' + str(id) + '_' + str(num) + '.jpg'
        cv2.imwrite(file, new_img)
        print('Saving ' + file)
