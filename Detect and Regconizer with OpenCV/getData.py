import cv2
import numpy as np
import sqlite3
import os

def insertOrUpdate(id,name):
    conn=sqlite3.connect(r'./recognizer/Detect_Recog_DB.db')
    query="SELECT * FROM PEOPLE WHERE ID="+str(id)
    cusror=conn.execute(query)
    isRecordExist=0
    for row in cusror:
        isRecordExist=1
    if (isRecordExist==0):
        query="INSERT INTO PEOPLE(ID,NAME) Values("+str(id)+",'"+str(name)+"')"
    else:
        query="UPDATE PEOPLE set NAME='"+str(name)+"' Where ID="+str(id)
    if ( conn.execute(query)):
        print(query)
    conn.commit()
    conn.close()

#load tv
face_cascade = cv2.CascadeClassifier(cv2.haarcascades+"haarcascade_frontalface_default.xml")
cap=cv2.VideoCapture(0)
#insert to DB
id=input("ENter your ID: ")
name=input("ENter your Name: ")
insertOrUpdate(id,name)
sampleNum=0
start=0
while(True):
    ret,frame=cap.read()
    gray=cv2.cvtColor(frame,cv2.COLOR_BGR2GRAY)
    faces=face_cascade.detectMultiScale(gray,1.3,5)
    
    for (x,y,w,h) in faces:
        cv2.rectangle(frame,(x,y),(x+w,y+h),(0,255,0),2)
        if (not os.path.exists('dataset')):
            os.makedirs('dataset')
        if (start==1):
            sampleNum+=1
            cv2.imwrite('dataset/User.'+str(id)+'.'+str(sampleNum)+'.jpg',gray[y:y+h,x:x+w])
            cv2.putText(frame, "Collecting: "+str(sampleNum)+"/100", (x,y-15+25), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0),2)     
    cv2.imshow('frame',frame)
    if (cv2.waitKey(20) & 0xFF==ord('b')): #press 'b' to BEGIN
        start=1
    if (sampleNum>100):
        cap.release()
        cv2.destroyAllWindows()
        break






