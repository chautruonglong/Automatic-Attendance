from os import makedirs
from os.path import exists, split
from src.myfacenet.detector import MTCNNDetector
from facenet.src.facenet import get_dataset, to_rgb
from numpy import asarray, vstack, argmax, sum, power, squeeze, zeros, int32
from numpy import maximum, minimum
from scipy.misc import imresize, imsave, imread
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from cv2 import GaussianBlur

INPUT_DATASET = '../dataset/raw/'
OUTPUT_DATASET = '../dataset/processed/'
MTCNN_MODEL = '../models/premodels/align'
FACE_SIZE = 140
MARGIN = 20
GPU_MEM_FRACTION = 0.3
MIN_SIZE = 20


def main():
    if not exists(OUTPUT_DATASET):
        makedirs(OUTPUT_DATASET)

    dataset = get_dataset(INPUT_DATASET)
    print(f'Found {len(dataset)} people in dataset')

    with Graph().as_default():
        gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
        sess = Session(config=ConfigProto(gpu_options=gpu_options, log_device_placement=False))

        with sess.as_default():
            detector = MTCNNDetector(sess, MTCNN_MODEL, MIN_SIZE)

            total_images = 0
            successfully_images = 0

            for data in dataset:
                img_paths = data.image_paths

                output_class_dir = OUTPUT_DATASET + data.name + '/'
                if not exists(output_class_dir):
                    makedirs(output_class_dir)

                for img_path in img_paths:
                    total_images += 1
                    print(f'Aligning {img_path}')

                    file_name = split(img_path)[-1]
                    output_file_dir = output_class_dir + file_name

                    try:
                        img = imread(img_path)
                    except EOFError as error:
                        print(f'{img_path} {error}')
                    else:
                        if img.ndim < 2:
                            print(f'Can not align {img_path}')
                            continue
                        elif img.ndim == 2:
                            img = to_rgb(img)
                            img = img[:, :, :3]

                        bboxes = detector.detect(img)
                        nrof_faces = bboxes.shape[0]

                        if nrof_faces > 0:
                            det = bboxes[:, :4]
                            det_arr = []
                            img_size = asarray(img.shape)[:2]

                            if nrof_faces > 1:
                                bbox_size = (det[:, 2] - det[:, 0]) * (det[:, 3] - det[:, 1])
                                img_center = img_size / 2
                                offsets = vstack([(det[:, 0] + det[:, 2]) / 2 - img_center[1], (det[:, 1] + det[:, 3]) / 2 - img_center[0]])
                                offset_dist_squared = sum(power(offsets, 2), 0)
                                index = argmax(bbox_size - offset_dist_squared * 2)
                                det_arr.append(det[index, :])
                            else:
                                det_arr.append(squeeze(det))

                            for index, det in enumerate(det_arr):
                                det = squeeze(det)
                                bb = zeros(4, dtype=int32)

                                bb[0] = maximum(det[0] - MARGIN / 2, 0)
                                bb[1] = maximum(det[1] - MARGIN / 2, 0)
                                bb[2] = minimum(det[2] + MARGIN / 2, img_size[1])
                                bb[3] = minimum(det[3] + MARGIN / 2, img_size[0])

                                cropped = img[bb[1]:bb[3], bb[0]:bb[2], :]
                                scaled = imresize(cropped, (FACE_SIZE, FACE_SIZE))
                                scaled = GaussianBlur(scaled, (3, 3), 0)

                                successfully_images += 1
                                imsave(output_file_dir, scaled)
                        else:
                            print(f'Can not align {img_path}')

            print(f'Total images found: {total_images}')
            print(f'Number of successfully aligned images: {successfully_images}')


if __name__ == '__main__':
    main()
    exit(0)
