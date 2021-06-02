import os

PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/data/1814/'

names = os.listdir(PATH)

for name in names:
    path = PATH + name + '/'
    
    files = os.listdir(path)
    files.sort(key=lambda s: int(s[s.index('_') + 1:s.index('.')]))
    
    count = 0
    
    for file in files:
        os.rename(path + file, path + name + '_' + str(count) + '.jpg')
        count += 1