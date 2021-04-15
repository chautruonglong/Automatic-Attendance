import os

INPUT_PATH = 'raw/h264/class/'
OUTPUT_PATH = 'raw/mp4/class/'

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