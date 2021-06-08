from datetime import datetime

from tensorflow import Graph, Session, ConfigProto

from backend_api.utils import convert_time, convert_date
from recognition.background_process import BackgroundProcess
from recognition.myfacenet.detector import HaarcascadeDetector
from recognition.myfacenet.encoder import FacenetEncoder
from recognition.myfacenet.identifier import FacenetIdentifier
from recognition.raspberry_process import RaspberryProcess
from recognition.window_process import WindowProcess

CLASSIFIER_MODEL = 'recognition/models/mymodels/1814_140s_128d_svm_big.pkl'
FACENET_MODEL = 'recognition/models/premodels/128/20170512-110547.pb'
MTCNN_MODEL = 'recognition/models/premodels/align'
HAARCASCADE_MODEL = 'recognition/models/premodels/haarcascade/haarcascade_frontalface_default.xml'

THRESHOLD = 70
GPU_MEM_FRACTION = 0.4
FACE_SIZE = 140
MIN_SIZE = 100


class AttendanceProcess:
    def __init__(self):
        with Graph().as_default():
            # gpu_options = GPUOptions(per_process_gpu_memory_fraction=GPU_MEM_FRACTION)
            self._sess = Session(config=ConfigProto(log_device_placement=False, device_count={'GPU': 0}))

            with self._sess.as_default():
                self._detector = HaarcascadeDetector(HAARCASCADE_MODEL, MIN_SIZE)
                self._encoder = FacenetEncoder(FACENET_MODEL, FACE_SIZE, self._sess)
                self._identifier = FacenetIdentifier(None, CLASSIFIER_MODEL)
                self._camera = None

    def attendance(self, process_id, subject_id, window=False, rasp=False, timeout=10, per=0.1):
        now = datetime.now()
        date = convert_date(now.date())
        time = convert_time(now.time())

        if rasp is True:
            print('A raspberry process is running')
            process = RaspberryProcess(process_id, subject_id, 0, self._detector, self._encoder, self._identifier)
            try:
                process.start(timeout, per, date, time, THRESHOLD)
            except Exception as error:
                process.close()
                raise error
            finally:
                process.close()

        else:
            if window is True:
                print('A window process is running')
                process = WindowProcess(process_id, subject_id, 1, self._detector, self._encoder, self._identifier)
                try:
                    process.start(timeout, per, date, time, THRESHOLD)
                except Exception as error:
                    process.close()
                    raise error
                finally:
                    process.close()

            else:
                print('A background process is running')
                process = BackgroundProcess(process_id, subject_id, 0, self._detector, self._encoder, self._identifier)
                try:
                    process.start(timeout, per, date, time, THRESHOLD)
                except Exception as error:
                    process.close()
                    raise error
                finally:
                    process.close()


attendance_process = AttendanceProcess()
