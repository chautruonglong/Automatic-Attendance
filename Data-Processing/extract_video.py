import os
import cv2

INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/raw/h264-fixed/1814/'
OUTPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'
HAARCASADE_PATH = '/home/chautruonglong/Desktop/Face-Recognition/models/premodels/haarcascade/haarcascade_frontalface_default.xml'

detector = cv2.CascadeClassifier(HAARCASADE_PATH)

files = os.listdir(INPUT_PATH)

for file in files:
    try: 
        name, ext = file.split('.')
        
        if ext == 'h264':
            
            path = OUTPUT_PATH + name
            if not os.path.exists(path):
                os.makedirs(path)
            
            video = cv2.VideoCapture(INPUT_PATH + file)
            count = len(os.listdir(path))

            while True:
                is_has, frame = video.read()
                
                if is_has:
                    faces = detector.detectMultiScale(frame, 1.2, 4)
                    
                    for x, y, w, h in faces:
                        new_img = frame[y:y + h, x:x + w]
                        cv2.imwrite(f'{path}/{name}_{count}.png', new_img)
                        print(f'Saving {path}/{name}_{count}.png')
                        count += 1 
                    
                    del frame
                else:
                    break
                
            del video
    except Exception as e:
        print(str(e))