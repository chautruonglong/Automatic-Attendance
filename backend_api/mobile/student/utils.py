from backend_api import settings
from backend_api.utils import convert_date
from backend_api.utils import get_base_url


class StudentParser:
    def __init__(self, request, student):
        self._student_id = student.student_id
        self._name = student.name
        self._phone = student.phone
        self._birth = convert_date(student.birth)
        self._class_name = student.class_name
        self._img_3x4 = f'{get_base_url(request)}/{settings.MEDIA_URL}{student.img_3x4}'

    def to_dict(self):
        return {
            'student_id': self._student_id,
            'name': self._name,
            'phone': self._phone,
            'birth': self._birth,
            "class_name": self._class_name,
            'img_3x4': self._img_3x4
        }
