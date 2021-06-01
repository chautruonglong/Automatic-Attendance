from time import sleep

from cv2 import flip, VideoCapture

from core.models import Process
from recognition.utils import save_unknown, save_attended


class BackgroundProcess:
    def __init__(self, process_id, subject_id, video, detector, encoder, identifier):
        self._camera = VideoCapture(video)
        if not self._camera.isOpened():
            raise Exception('Camera is not open')

        self._process_id = process_id
        self._subject_id = subject_id
        self._detector = detector
        self._encoder = encoder
        self._identifier = identifier

    def start(self, timeout, per, date, time, threshold):
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
                    face_embedding = self._encoder.encode_face(img_face)
                    student_id, confidence = self._identifier.identify(face_embedding)

                    confidence *= 100
                    confidence = round(confidence, 1)

                    if confidence > threshold:
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
                        save_unknown(self._process_id, self._subject_id, img_face, confidence, date, time)

            timeout -= per
            sleep(per)

        for student_id in students:
            save_attended(self._process_id, self._subject_id, student_id, students[student_id]['img_face'],
                          students[student_id]['confidence'], date, time)

        Process.objects.update_or_create(
            process_id=self._process_id,
            defaults={
                'state': True
            }
        )

    def close(self):
        self._camera.release()
