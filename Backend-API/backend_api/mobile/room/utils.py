class RoomParser:
    def __init__(self):
        self._room_dict = dict()

    def add(self, zone, room_id):
        if zone in self._room_dict.keys():
            self._room_dict[zone].append(room_id)
        else:
            self._room_dict[zone] = [room_id]

    def to_dict(self):
        return self._room_dict
