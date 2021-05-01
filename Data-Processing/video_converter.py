import os

INPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/raw/h264-fixed/1814/'
OUTPUT_PATH = '/media/chautruonglong/Data/University/Nam-3/Ky-2/PBL5-ky-thuat-may-tinh/Main-project/Main-Dataset/raw/mp4/1814/'

def command(file):
    name = file[: file.index('.')]
    return 'ffmpeg -y -i ' + INPUT_PATH + name + '.h264 -c copy ' + OUTPUT_PATH + name + '.mp4 >/dev/null 2>&1'

if not os.path.exists(OUTPUT_PATH):
    os.makedirs(OUTPUT_PATH)
    
for file in os.listdir(INPUT_PATH):
    extension = file.split('.')[-1]
    
    if extension == 'h264':
        cmd = command(file)
        print('Excuting ' + cmd)
        os.system(cmd)