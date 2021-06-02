from torch import device, load, from_numpy, no_grad
from enhancer.rrdbnet_arch import RRDBNet
from numpy import transpose


class ImageEnhancer:
    def __init__(self, esrgan_model):
        self._dev = device('cuda')
        self._model = RRDBNet(3, 3, 64, 23, gc=32)
        self._model.load_state_dict(load(esrgan_model), strict=True)
        self._model.eval()
        self._model = self._model.to(self._dev)

        print(f'Using ESRGAN model: {esrgan_model}')

    def enhance(self, img):
        img = img * 1.0 / 255
        img = from_numpy(transpose(img[:, :, [2, 1, 0]], (2, 0, 1))).float()
        img_LR = img.unsqueeze(0)
        img_LR = img_LR.to(self._dev)

        with no_grad():
            output = self._model(img_LR).data.squeeze().float().cpu().clamp_(0, 1).numpy()

        output = transpose(output[[2, 1, 0], :, :], (1, 2, 0))
        output = (output * 255.0).round()

        return output
