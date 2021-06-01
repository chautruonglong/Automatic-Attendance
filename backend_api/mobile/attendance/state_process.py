from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from backend_api.utils import convert_date
from core.models import Process


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def state_process_api(request, process_id):
    try:
        process = Process.objects.get(process_id=process_id)

        return Response(
            data={
                'state': process.state,
                'date': convert_date(process.date),
                'time': process.time
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