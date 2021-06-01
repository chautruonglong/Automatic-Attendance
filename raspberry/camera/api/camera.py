from cv2 import VideoCapture, imencode
from base64 import b64encode


class Camera:
    _camera = None
    _is_open = False

    def open(self):
        if self._camera is None or not self._camera.grab():
            self._camera = VideoCapture(1)

            if not self._camera.isOpened():
                raise Exception('Camera is not open')
        else:
            raise Exception('Camera is using right now')

        self._is_open = True

    def close(self):
        if self._camera is not None:
            self._camera.release()
            self._camera = None
            self._is_open = False

    def capture(self):
        if self._is_open is False:
            raise Exception('Cannot capture, camera is not open')

        ret, frame = self._camera.read()
        ret, buffer = imencode('.jpg', frame)
        jpg_text = b64encode(buffer)
        return jpg_text


camera = Camera()
