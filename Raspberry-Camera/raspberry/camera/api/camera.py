from cv2 import VideoCapture, imencode
from base64 import b64encode
from raspberry.leb import Led


class Camera:
    _camera = None
    _is_open = False
    _led = Led()

    def open(self, video):
        if self._is_open is False:
            self._camera = VideoCapture(video)

            if not self._camera.isOpened():
                raise Exception('Camera is not open')
            
            self._led.turn_on()
            self._is_open = True
        else:
            raise Exception('Camera is using right now')

    def close(self):
        if self._camera is not None:
            self._camera.release()
            self._camera = None
            self._led.turn_off()
            self._is_open = False

    def capture(self):
        if self._is_open is False:
            raise Exception('Cannot capture, camera is not open')

        ret, frame = self._camera.read()
        ret, buffer = imencode('.jpg', frame)
        jpg_text = b64encode(buffer)
        return jpg_text


camera = Camera()
