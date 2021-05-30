from core.models import Unknown, Attendance, StudentSubject, Process
from cv2 import imwrite
from backend_api.utils import convert_time
from backend_api.settings import MEDIA_URL
from datetime import datetime
from django.utils import six
from django.utils.crypto import salted_hmac
import os


def save_attended(process_id, subject_id, student_id, img_face, confidence, date, time):
    try:
        stusub = StudentSubject.objects.get(
            student_id=student_id,
            subject_id=subject_id
        )

        url = f'{MEDIA_URL}images/attendances/{subject_id}/{date}/{time}/knows'
        if not os.path.exists(url):
            os.makedirs(url)

        file_name = f'{url}/{student_id}.jpg'
        imwrite(file_name, img_face)

        Attendance.objects.create(
            stusub_id=stusub.stusub_id,
            process_id=process_id,
            img_face=file_name[len(MEDIA_URL):],
            confidence=confidence
        )
    except Exception as error:
        print('Saving unknown person:', str(error))
        save_unknown(process_id, subject_id, img_face, confidence, date, time)


def save_unknown(process_id, subject_id, img_face, confidence, date, time):
    now = datetime.now()
    unknown_id = generate_unknown_id(process_id, now.date(), now.time())

    url = f'{MEDIA_URL}images/attendances/{subject_id}/{date}/{time}/unknowns'
    if not os.path.exists(url):
        os.makedirs(url)

    file_name = f'{url}/{unknown_id}.jpg'
    imwrite(file_name, img_face)

    Unknown.objects.create(
        unknown_id=unknown_id,
        process_id=process_id,
        img_face=file_name[len(MEDIA_URL):],
        confidence=confidence
    )


def generate_unknown_id(process_id, date, time, size=255):
    KEY_SALT = 'unknown.id'
    value = six.text_type(process_id) + six.text_type(date) + six.text_type(time)
    hash = salted_hmac(KEY_SALT, value)
    return hash.hexdigest()[:size]
