from sys import argv
from torch import save, load
from collections import OrderedDict

NET_PSNR_PATH = 'models/premodels/esrgan/RRDB_PSNR_x4.pth'
NET_ESRGAN_PATH = 'models/premodels/esrgan/RRDB_ESRGAN_x4.pth'

if __name__ == '__main__':
    alpha = float(argv[1])
    net_interp_path = 'models/premodels/esrgan/interp_{:02d}.pth'.format(int(alpha * 10))

    print('Interpolating with alpha = ', alpha)

    net_psnr = load(NET_PSNR_PATH)
    net_esrgan = load(NET_ESRGAN_PATH)
    net_interp = OrderedDict()

    for k, v_psnr in net_psnr.items():
        v_esrgan = net_esrgan[k]
        net_interp[k] = (1 - alpha) * v_psnr + alpha * v_esrgan

    save(net_interp, net_interp_path)
