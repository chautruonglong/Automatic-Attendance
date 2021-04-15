from numpy import expand_dims
from keras.preprocessing.image import load_img, img_to_array, save_img
from keras.preprocessing.image import ImageDataGenerator
import os, gc

N_STUDENTS = 20
N_IMGS = 40

student_id = 102180160

generator = ImageDataGenerator(width_shift_range=[-50, 50],
                               height_shift_range=0.3,
                               horizontal_flip=True,
                               vertical_flip=True,
                               rotation_range=50,
                               brightness_range=[0.2, 1.3],
                               zoom_range=[0.5, 1.3],
                               fill_mode='nearest'
                               )

for i in range(N_STUDENTS):
    try:
        student_id += 1
        img = load_img('K18/' + str(student_id) + '.jpg')
        data = img_to_array(img)
        data = expand_dims(data, axis=0)
        
        iterator = generator.flow(data, batch_size=1)
        
        folder = 'Data-Gen/' + str(student_id)
        if not os.path.exists(folder):
            os.mkdir(folder)
                
        for j in range(N_IMGS):
            path = folder + '/' + str(student_id) + '_' + str(j) + '.jpg'
            batch = iterator.next()
            new_img = batch[0].astype('uint8')
            
            print('Save ' + path)
            save_img(path, new_img)
            
            del path, batch, new_img
        
        del img, data, iterator, folder
        gc.collect()
    except:
        continue

print('========================DONE=======================')