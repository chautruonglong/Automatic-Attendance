import os
import cv2

INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/raw/h264-fixed/1814/'
OUTPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'
HAARCASADE_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Automatic-Attendance/Face-Recognition/premodels/haarcascade_frontalface_default.xml'

detector = cv2.CascadeClassifier(HAARCASADE_PATH)

for file in os.listdir(INPUT_PATH):
    try: 
        name, ext = file.split('.')
        
        if ext == 'h264':
            
            path = OUTPUT_PATH + name
            if not os.path.exists(path):
                os.makedirs(path)
            
            video = cv2.VideoCapture(INPUT_PATH + file)
            count = 0

            while True:
                is_has, frame = video.read()
                
                if is_has:
                    faces = detector.detectMultiScale(frame, 1.3, 5)
                    
                    for x, y, w, h in faces:
                        new_img = frame[y:y + h, x:x + w]
                        cv2.imwrite(f'{path}/{name}_{count}.jpg', new_img)
                        print(f'Saving {path}/{name}_{count}.jpg')
                        count += 1 
                else:
                    break
                
                del frame
                
            del video
    except:
        pass