# from multiprocessing import Process
# import cv2
# import time
#
#
# def func(img, i, timeout=50, per=0.1, window=True):
#     if window is True:
#         cv2.namedWindow(f'w{i}', cv2.WINDOW_NORMAL)
#         capture = cv2.VideoCapture(1)
#     while True:
#         ret, img = capture.read()
#         if window is True:
#             cv2.imshow(f'w{i}', img)
#             cv2.waitKey(1)
#
#         if timeout == 0:
#             break
#
#         timeout -= per
#         time.sleep(per)
#
#     if window is True:
#         cv2.destroyWindow(f'w{i}')
#     print(i)
#
#
# img = cv2.imread('resources/images/originals/it/k18/102180153.jpg')
#
# for i in range(1):
#     process = Process(target=func, args=(img, i))
#     process.start()
print(zip(dict(1)))