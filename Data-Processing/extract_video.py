import os
import cv2

INPUT_PATH = 'raw/mp4/class/'
OUTPUT_PATH = 'data/class/'

cv2.VideoCapture()
for file in os.listdir(INPUT_PATH):
    try: 
        name, ext = file.split('.')
        
        if ext == 'mp4':
            path = OUTPUT_PATH + name
            if not os.path.exists(path):
                os.makedirs(path)
            
            video = cv2.VideoCapture(INPUT_PATH + file)
            count = 0
            
            while True:
                is_has, frame = video.read()
                
                if is_has:
                    new_img = f'{path}/{name}_{count}.jpg'
                    cv2.imwrite(new_img, frame)
                    count += 1
                    print(f'Saving {new_img}')
                    
                    del frame, is_has
                else:
                    break
        
        video.release()
        del video, name, ext
    except:
        pass

cv2.destroyAllWindows()