import cv2
import os
import numpy as np
import sqlite3
from PIL import Image

#doc file yml da regconize va so khop voi face_cascade
face_cascade=cv2.CascadeClassifier(cv2.data.haarcascades+"haarcascade_frontalface_default.xml")
recognizer=cv2.face.LBPHFaceRecognizer_create()

recognizer_Path=r'./trainingData.yml'
print(recognizer_Path)
recognizer.read(recognizer_Path)

#lay du lieu tu DB
def getProfile(id):
    conn=sqlite3.connect(r'./Detect_Recog_DB.db')
    query="SELECT * FROM PEOPLE WHERE ID="+str(id)
    cursor=conn.execute(query)

    profile=None
    for row in cursor:
        profile=row
    conn.close
    return profile

cap=cv2.VideoCapture(0)
while(True):
    ret,frame=cap.read()
    gray =cv2.cvtColor(frame,cv2.COLOR_BGR2GRAY)
    faces=face_cascade.detectMultiScale(gray)
    for (x,y,w,h) in faces:
        cv2.rectangle(frame,(x,y),(x+w,y+h),(255,0,0),2)
        roi_gray=gray[y:y+h,x:x+w]
        id, confidence= recognizer.predict(roi_gray)
        if confidence<40:
            profile=getProfile(id)
            if (profile!=None):
                cv2.rectangle(frame,(x,y),(x+w,y+h),(0,255,0),2)
                confidence="{0}%".format(round(100-confidence))
                profile=profile[1]         
        else:
                cv2.rectangle(frame,(x,y),(x+w,y+h),(0,0,255),2)
                confidence="{0}%".format(round(100-confidence))
                profile="Unknown"
        cv2.putText(frame, profile, (x,y-15), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0),2)
        cv2.putText(frame, confidence, (x,y-15+25), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0),2)     
    cv2.imshow('Recognize ',frame)
    if(cv2.waitKey(1) & 0xFF==ord('q')):
        break   
cap.release()
cv2.destroyAllWindows()
