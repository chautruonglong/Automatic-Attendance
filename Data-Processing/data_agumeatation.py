from numpy import expand_dims
from keras.preprocessing.image import load_img, img_to_array, save_img
from keras.preprocessing.image import ImageDataGenerator
import os
import numpy as np
import cv2

PATH = '/home/chautruonglong/Desktop/Face-Recognition/dataset/raw/'

def img_resize(img):
    w, h, c = img.shape
    r = np.random.randint(1, 20)
    w = int(w * r / 10)
    h = int(h * r / 10)
    if w >= 25 and h >= 25:
        img = cv2.resize(img, dsize=(w, h), interpolation=cv2.INTER_LANCZOS4)
    return img

generator = ImageDataGenerator(
    horizontal_flip=True,
    rotation_range=15,
    brightness_range=[0.2, 1.8],
    fill_mode='constant'
)

names = os.listdir(PATH)

for name in names:
    path = PATH + name + '/'
    
    files = os.listdir(path)

    for file in files:
        file_name, ext = file.split('.')

        img = load_img(path + file)
        data = img_to_array(img)
        data = expand_dims(data, axis=0)
        
        iterator = generator.flow(data, batch_size=64)
        
        num = 0
        for i in range(10):
            file_path = path + file_name + '_' + str(num) + '.png'
    
            batch = iterator.next()
            new_img = batch[0].astype('uint8')
            
            print('Saving ' + file_path)
            save_img(file_path, new_img)
            num += 1
        
        for i in range(10):
            file_path = path + file_name + '_' + str(num) + '.png'
    
            batch = iterator.next()
            new_img = batch[0].astype('uint8')
            new_img = img_resize(new_img)
            
            print('Saving ' + file_path)
            save_img(file_path, new_img)
            num += 1
            

print('========================DONE=======================')