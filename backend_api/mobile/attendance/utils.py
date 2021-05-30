from backend_api.utils import get_base_url
from backend_api.settings import MEDIA_URL


class AttendanceParser:
    def __init__(self, request, type, id, img_face, confidence):
        self._type = type
        self._id = id
        self._img_face = f'{get_base_url(request)}/{MEDIA_URL}{img_face}'
        self._confidence = confidence

    def to_dict(self):
        return {
            'type': self._type,
            'id': self._id,
            'img_face': self._img_face,
            'confidence': self._confidence
        }
