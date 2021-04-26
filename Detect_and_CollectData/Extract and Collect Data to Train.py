import os
import cv2
import numpy as np
def Getdata(stringPath):
    face_cascade = cv2.CascadeClassifier(cv2.haarcascades+"haarcascade_frontalface_default.xml")
    s='\\'
    user_name= stringPath.split('1814')[1].split(s)[1][:-5]
    #cap=cv2.VideoCapture("D:\\Nam3_HK2\\1814\\"+user_name+".h264")
    cap=cv2.VideoCapture(stringPath)
    user_folder="data_"+user_name
    if (not os.path.exists(user_folder)):
        os.makedirs(user_folder)
    sampleNum=0
    while(cap.isOpened()):
        try: 
            ret,frame=cap.read()
            faces=face_cascade.detectMultiScale(frame)
            for (x,y,w,h) in faces:
                cv2.rectangle(frame,(x,y),(x+w,y+h),(0,0,255),2)
                sampleNum+=1
                cv2.imwrite(user_folder+'/'+user_name+'_'+str(sampleNum)+'.jpg',frame[y:y+h,x:x+w])
            cv2.imshow("Extract and Collect Data to Train",frame)
        except:
            break
        if(cv2.waitKey(1)==ord('q')):
            break
    cap.release()
    cv2.destroyAllWindows()

path='1814'
imagePaths=[os.path.join(path,f) for f in os.listdir(path)]
for imagepath in imagePaths:
    Getdata(imagepath)
