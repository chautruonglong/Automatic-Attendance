"""
:author chautruonglong
"""

from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Room


@api_view(['POST'])
@permission_classes((HasAPIKey, ))
def create_room_api(request):
    try:
        body = request.data
        room_id = body['room_id'].upper()
        zone = room_id[0]
        ip_cam = body['ip_cam']

        Room.objects.create(room_id=room_id, zone=zone, ip_cam=ip_cam)

        return Response(
            data={
                'message': 'Create successfully'
            },
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
