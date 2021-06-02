import os

PATH = '/home/chautruonglong/Desktop/Face-Recognition/dataset/raw/'

names = os.listdir(PATH)

for name in names:
    path = PATH + name + '/'
    files = os.listdir(path)
    
    for file in files:
        c = file.count('r')
        if c == 1:
            os.remove(path + file)