import cv2
import numpy as np
import os
from PIL import Image
 
recognizer=cv2.face.LBPHFaceRecognizer_create()
path='dataset'
def getImageWithId(path):
    imagePaths=[os.path.join(path,f) for f in os.listdir(path)]
    faces=[]
    IDs=[]
    for imagePath in imagePaths:
        faceImg=Image.open(imagePath).convert('L')
        faceNp=np.array(faceImg,'uint8')
        s='\\'
        Id= int(imagePath.split(s)[1].split('.')[1])
        faces.append(faceNp)
        IDs.append(Id)
        cv2.imshow('training',faceNp)
        print(imagePath)
        cv2.waitKey(10)
    return faces,IDs
faces, IDs=getImageWithId(path)
recognizer.train(faces,np.array(IDs))
if not os.path.exists('recognizer'):
    os.makedirs('recognizer')
recognizer.save('./recognizer/trainingData.yml')
cv2.destroyAllWindows()
