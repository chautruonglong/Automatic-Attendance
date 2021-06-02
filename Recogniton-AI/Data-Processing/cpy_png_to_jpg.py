import cv2
import os

PATH = '/home/chautruonglong/Desktop/Face-Recognition/dataset/raw/'

names = os.listdir(PATH)

for name in names:
    path = PATH + name + '/'
    files = os.listdir(path)
    
    for file in files:
        f_name, ext = file.split('.')
        
        img = cv2.imread(path + file)
        cv2.imwrite(path + f_name + '_j.jpg', img)
        print(f'Saving ' + path + f_name + '_j.jpg')