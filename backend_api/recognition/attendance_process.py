from threading import Thread
from recognition.myfacenet.encoder import FacenetEncoder
from recognition.myfacenet.detector import HaarcascadeDetector
from recognition.myfacenet.identifier import FacenetIdentifier
from recognition.utils import save_unknown, save_attended
from tensorflow import Graph, Session, ConfigProto, GPUOptions
from time import sleep
from cv2 import imshow, imwrite, WINDOW_NORMAL, namedWindow
from cv2 import waitKey, destroyWindow, VideoCapture, flip
from cv2 import rectangle, putText, FONT_HERSHEY_COMPLEX_SMALL
from cv2 import resizeWindow
from numpy import uint8, array
from datetime import datetime
from backend_api.utils import convert_time, convert_date
from core.models import Process as ProcessModel


CLASSIFIER_MODEL = 'recognition/models/mymodels/1814_140s_128d_svm_small.pkl'
FACENET_MODEL = 'recognition/models/premodels/128/20170512-110547.pb'
MTCNN_MODEL = 'recognition/models/premodels/align'
HAARCASCADE_MODEL = 'recognition/models/premodels/haarcascade/haarcascade_frontalface_default.xml'

THRESHOLD = 70
GPU_MEM_FRACTION = 0.4
FACE_SIZE = 140
MIN_SIZE = 70


class AttendanceProcess:
    def __init__(self):
        with Graph().as_default():
            # gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
            self._sess = Session(config=ConfigProto(log_device_placement=False, device_count={'GPU': 0}))

            with self._sess.as_default():
                self._detector = HaarcascadeDetector(HAARCASCADE_MODEL, MIN_SIZE)
                self._encoder = FacenetEncoder(FACENET_MODEL, FACE_SIZE)
                self._identifier = FacenetIdentifier(None, CLASSIFIER_MODEL)
                self._camera = None

    def attendance(self, process_id, subject_id, window=False, timeout=10, per=0.1):
        if self._camera is None or not self._camera.grab():
            self._camera = VideoCapture(1)

            if not self._camera.isOpened():
                raise Exception('Camera is not open')
        else:
            raise Exception('Camera is using right now')

        now = datetime.now()
        date = convert_date(now.date())
        time = convert_time(now.time())

        if window is True:
            print('A window process is running')
            # self._window_process(process_id, subject_id, timeout, per, date, time)
            process = Thread(target=self._window_process, args=(process_id, subject_id, timeout, per, date, time))
            process.start()

        else:
            print('A background process is running')
            self._background_process(process_id, subject_id, timeout, per, date, time)
            # process = Thread(target=self._background_process, args=(process_id, subject_id, timeout, per, date, time))
            # process.start()

    def _window_process(self, process_id, subject_id, timeout, per, date, time):
        namedWindow('camera', WINDOW_NORMAL)
        students = dict()
        while timeout > 0:
            ret, frame = self._camera.read()
            frame = flip(frame, 1)
            faces = self._detector.detect(frame)

            for face in faces:
                x1 = int(face[0])
                y1 = int(face[1])
                x2 = int(face[2])
                y2 = int(face[3])

                if x1 > 0 and y1 > 0 and x2 > 0 and y2 > 0:
                    img_face = frame[y1:y2, x1:x2]
                    face_embedding = self._encoder.encode_face(self._sess, img_face)
                    student_id, confidence = self._identifier.identify(face_embedding)

                    confidence *= 100
                    confidence = round(confidence, 1)

                    if confidence > THRESHOLD:
                        self._put_face_label(frame, x1, y1, x2, y2, student_id, confidence)
                        if student_id not in students.keys():
                            print(f'Student_id: {student_id}, confidence: {confidence}')
                            students[student_id] = dict()
                            students[student_id]['confidence'] = confidence
                            students[student_id]['img_face'] = img_face

                        elif confidence > students[student_id]['confidence']:
                            print(f'Replace student_id: {student_id}, confidence: {confidence}')
                            students[student_id]['confidence'] = confidence
                            students[student_id]['img_face'] = img_face

                    else:
                        self._put_face_label(frame, x1, y1, x2, y2, 'Unknown', '')
                        save_unknown(process_id, subject_id, img_face, confidence, date, time)

            imshow('camera', frame)
            waitKey(1)

            timeout -= per
            sleep(per)

        for student_id in students:
            save_attended(process_id, subject_id, student_id, students[student_id]['img_face'], students[student_id]['confidence'], date, time)

        ProcessModel.objects.update_or_create(
            process_id=process_id,
            defaults={
                'state': True
            }
        )

        self._camera.release()
        destroyWindow('camera')

    def _background_process(self, process_id, subject_id, timeout, per, date, time):
        students = dict()
        while timeout > 0:
            ret, frame = self._camera.read()
            frame = flip(frame, 1)
            faces = self._detector.detect(frame)

            for face in faces:
                x1 = int(face[0])
                y1 = int(face[1])
                x2 = int(face[2])
                y2 = int(face[3])

                if x1 > 0 and y1 > 0 and x2 > 0 and y2 > 0:
                    img_face = frame[y1:y2, x1:x2]
                    face_embedding = self._encoder.encode_face(self._sess, img_face)
                    student_id, confidence = self._identifier.identify(face_embedding)

                    confidence *= 100
                    confidence = round(confidence, 1)

                    if confidence > THRESHOLD:
                        self._put_face_label(frame, x1, y1, x2, y2, student_id, confidence)
                        if student_id not in students.keys():
                            print(f'Student_id: {student_id}, confidence: {confidence}')
                            students[student_id] = dict()
                            students[student_id]['confidence'] = confidence
                            students[student_id]['img_face'] = img_face

                        elif confidence > students[student_id]['confidence']:
                            print(f'Replace student_id: {student_id}, confidence: {confidence}')
                            students[student_id]['confidence'] = confidence
                            students[student_id]['img_face'] = img_face

                    else:
                        self._put_face_label(frame, x1, y1, x2, y2, 'Unknown', '')
                        save_unknown(process_id, subject_id, img_face, confidence, date, time)

            timeout -= per
            sleep(per)

        for student_id in students:
            save_attended(process_id, subject_id, student_id, students[student_id]['img_face'], students[student_id]['confidence'], date, time)

        ProcessModel.objects.update_or_create(
            process_id=process_id,
            defaults={
                'state': True
            }
        )

        self._camera.release()

    def _put_face_label(self, frame, x1, y1, x2, y2, student_id, confidence):
        rectangle(
            img=frame,
            pt1=(x1, y1),
            pt2=(x2, y2),
            color=(0, 255, 0),
            thickness=2
        )

        putText(
            img=frame,
            text=str(student_id),
            org=(x1, y2 + 20),
            fontFace=FONT_HERSHEY_COMPLEX_SMALL,
            fontScale=1,
            color=(255, 255, 255),
            thickness=1,
            lineType=2
        )

        putText(
            img=frame,
            text=str(confidence),
            org=(x1, y2 + 40),
            fontFace=FONT_HERSHEY_COMPLEX_SMALL,
            fontScale=1,
            color=(255, 255, 255),
            thickness=1,
            lineType=2
        )


attendance_process = AttendanceProcess()
