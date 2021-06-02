from os.path import splitext, basename
from glob import glob
from cv2 import imwrite, imread, IMREAD_COLOR, resize, INTER_LANCZOS4
from numpy import transpose
from torch import device, load, from_numpy, no_grad
from rrdbnet_arch import RRDBNet

INPUT_MODEL = '../../models/premodels/esrgan/interp_10.pth'
INPUT_IMG = '102180183_0.png'
OUTPUT_IMG = ''

if __name__ == '__main__':
    dev = device('cpu')

    model = RRDBNet(3, 3, 64, 23, gc=32)
    model.load_state_dict(load(INPUT_MODEL), strict=True)
    model.eval()
    model = model.to(dev)

    print('Model path: {:s} \nTesting...'.format(INPUT_MODEL))

    idx = 0
    for path in glob(INPUT_IMG):
        idx += 1
        base = splitext(basename(path))[0]
        print(idx, base)

        img = imread(path, IMREAD_COLOR)
        img = resize(img, (140, 140), interpolation=INTER_LANCZOS4)
        img = img * 1.0 / 255
        img = from_numpy(transpose(img[:, :, [2, 1, 0]], (2, 0, 1))).float()
        img_LR = img.unsqueeze(0)
        img_LR = img_LR.to(dev)

        with no_grad():
            output = model(img_LR).data.squeeze().float().cpu().clamp_(0, 1).numpy()

        output = transpose(output[[2, 1, 0], :, :], (1, 2, 0))
        output = (output * 255.0).round()
        imwrite(OUTPUT_IMG + '{:s}_rlt.png'.format(base), output)
