class SubjectParser:
    def __init__(self, subject):
        self._subject_id = subject.subject_id
        self._room_id = subject.room_id
        self._name = subject.name
        self._time_slot = subject.time_slot
        self._day = subject.day

    def to_dict(self):
        return {
            'subject_id': self._subject_id,
            'room_id': self._room_id,
            'name': self._name,
            'time_slot': self._time_slot,
            'day': self._day
        }
