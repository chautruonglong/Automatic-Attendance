from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework_api_key.permissions import HasAPIKey
from core.models import Room
from mobile.room.utils import RoomParser


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def list_room_api(request):
    try:
        rooms = Room.objects.all()

        data_json = []
        for room in rooms:
            data_json.append(
                {
                    'room_id': room.room_id
                }
            )

        return Response(
            data=data_json,
            status=status.HTTP_202_ACCEPTED
        )

    except Exception as error:
        print(str(error))
        return Response(
            data={
                'error_message': str(error)
            },
            status=status.HTTP_406_NOT_ACCEPTABLE
        )
