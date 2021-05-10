import os

PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'

names = os.listdir(PATH)

for name in names:
    path = PATH + name + '/'
    files = os.listdir(path)
    
    for file in files:
        c = file.count('_')
        if c == 2:
            os.remove(path + file)