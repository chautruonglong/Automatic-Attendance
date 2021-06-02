import os
import cv2
import numpy as np

PATH = '/home/chautruonglong/Desktop/Face-Recognition/dataset/raw/'

names = os.listdir(PATH)

for name in names:
    files = os.listdir(PATH + name)
    
    for file in files:
        file_name, ext = file.split('.')
        
        img = cv2.imread(PATH + name + '/' + file)
        w, h, z = img.shape
        
        count = 0
        for i in range(16, 21, 1):
            new_w = int(w * i / 10) + np.random.randint(-5, 20)
            new_h = int(h * i / 10) + np.random.randint(-5, 20)
            
            if new_w >= 15 and new_h >= 15:
                new_img = cv2.resize(img, (new_w, new_h))
                new_file = PATH + name + '/' + file_name + '_r_' + str(count) + '.png'
                cv2.imwrite(new_file, new_img)
                print(f'Saving {new_file}')
                count += 1
        
        for i in range(5):
            new_w = int(w * 0.1) + np.random.randint(-5, 20)
            new_h = int(h * 0.1) + np.random.randint(-5, 20)
            
            if new_w >= 15 and new_h >= 15:
                new_img = cv2.resize(img, (new_w, new_h))
                new_file = PATH + name + '/' + file_name + '_r_' + str(count) + '.png'
                cv2.imwrite(new_file, new_img)
                print(f'Saving {new_file}')
                count += 1
            