from numpy import expand_dims
from keras.preprocessing.image import load_img, img_to_array, save_img
from keras.preprocessing.image import ImageDataGenerator
import os, gc

N_IMGS = 50

OUTPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/faces/'
INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Dataset-3x4/Cong-nghe-thong-tin/K18/'


generator = ImageDataGenerator(width_shift_range=[-20, 30],
                               height_shift_range=0.3,
                               horizontal_flip=True,
                               vertical_flip=True,
                               rotation_range=30,
                               brightness_range=[0.2, 1.3],
                               zoom_range=[0.5, 1.3],
                               fill_mode='nearest'
                               )

names = os.listdir(OUTPUT_PATH)

for name in names:
    try:
        img = load_img(INPUT_PATH + name + '.jpg')
        data = img_to_array(img)
        data = expand_dims(data, axis=0)
        
        iterator = generator.flow(data, batch_size=1)
       
        folder = OUTPUT_PATH + name
        if not os.path.exists(folder):
            os.mkdir(folder)
            
        num = len(os.listdir(OUTPUT_PATH + name))
                
        for i in range(N_IMGS):
            path = folder + '/' + name + '_' + str(num + i) + '.jpg'
            batch = iterator.next()
            new_img = batch[0].astype('uint8')
            
            print('Saving ' + path)
            save_img(path, new_img)
            
            del path, batch, new_img
        
        del img, data, iterator, folder
        gc.collect()
        
    except:
        continue

print('========================DONE=======================')