from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Subject


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def list_time_slot_api(request, room_id, day):
    try:
        subjects = Subject.objects.filter(room_id=room_id, day=day)

        data_json = []
        for subject in subjects:
            data_json.append(subject.time_slot)

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
