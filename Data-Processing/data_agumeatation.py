from numpy import expand_dims
from keras.preprocessing.image import load_img, img_to_array, save_img
from keras.preprocessing.image import ImageDataGenerator
import os
import numpy as np
import random

N_IMGS = 10

PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'

def add_noise(img):
    VARIABILITY = 0.9
    deviation = VARIABILITY * random.random()
    noise = np.random.normal(0, deviation, img.shape)
    img += noise
    np.clip(img, 0., 255.)
    return img

generator = ImageDataGenerator(
    horizontal_flip=True,
    rotation_range=25,
    brightness_range=[0.3, 1.8],
    preprocessing_function=add_noise,
    fill_mode='nearest'
)

names = os.listdir(PATH)

for name in names:
    try:
        path = PATH + name + '/'
        
        files = os.listdir(path)

        for file in files:
            file_name, ext = file.split('.')
  
            img = load_img(path + file)
            data = img_to_array(img)
            data = expand_dims(data, axis=0)
            
            iterator = generator.flow(data, batch_size=64)
                    
            for i in range(N_IMGS):
                file_path = path + file_name + '_' + str(i) + '.jpg'
        
                batch = iterator.next()
                new_img = batch[0].astype('uint8')
                
                print('Saving ' + file_path)
                save_img(file_path, new_img)
            
    except Exception as e:
        print(str(e))

print('========================DONE=======================')