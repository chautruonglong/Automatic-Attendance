from torch import load, save
from rrdbnet_arch import RRDBNet

NET_ESRGAN_PATH = 'models/premodels/esrgan/RRDB_ESRGAN_x4.pth'

if __name__ == '__main__':
    pretrained_net = load(NET_ESRGAN_PATH)

    crt_model = RRDBNet(3, 3, 64, 23, gc=32)
    crt_net = crt_model.state_dict()

    load_net_clean = {}
    for k, v in pretrained_net.items():
        if k.startswith('module.'):
            load_net_clean[k[7:]] = v
        else:
            load_net_clean[k] = v
    pretrained_net = load_net_clean

    print('###################################\n')
    tbd = []
    for k, v in crt_net.items():
        tbd.append(k)

    for k, v in crt_net.items():
        if k in pretrained_net and pretrained_net[k].size() == v.size():
            crt_net[k] = pretrained_net[k]
            tbd.remove(k)

    crt_net['conv_first.weight'] = pretrained_net['model.0.weight']
    crt_net['conv_first.bias'] = pretrained_net['model.0.bias']

    for k in tbd.copy():
        if 'RDB' in k:
            ori_k = k.replace('RRDB_trunk.', 'model.1.sub.')
            if '.weight' in k:
                ori_k = ori_k.replace('.weight', '.0.weight')
            elif '.bias' in k:
                ori_k = ori_k.replace('.bias', '.0.bias')
            crt_net[k] = pretrained_net[ori_k]
            tbd.remove(k)

    crt_net['trunk_conv.weight'] = pretrained_net['model.1.sub.23.weight']
    crt_net['trunk_conv.bias'] = pretrained_net['model.1.sub.23.bias']
    crt_net['upconv1.weight'] = pretrained_net['model.3.weight']
    crt_net['upconv1.bias'] = pretrained_net['model.3.bias']
    crt_net['upconv2.weight'] = pretrained_net['model.6.weight']
    crt_net['upconv2.bias'] = pretrained_net['model.6.bias']
    crt_net['HRconv.weight'] = pretrained_net['model.8.weight']
    crt_net['HRconv.bias'] = pretrained_net['model.8.bias']
    crt_net['conv_last.weight'] = pretrained_net['model.10.weight']
    crt_net['conv_last.bias'] = pretrained_net['model.10.bias']

    print('Saving to ', NET_ESRGAN_PATH)
    save(crt_net, NET_ESRGAN_PATH)
