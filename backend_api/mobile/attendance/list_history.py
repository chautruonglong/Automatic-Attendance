from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework_api_key.permissions import HasAPIKey
from core.models import Process
from backend_api.utils import convert_date


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def list_history_api(request, subject_id):
    try:
        processes = Process.objects.filter(subject_id=subject_id)

        data_json = []
        for process in processes:
            if process.state is False:
                data_json.append(
                    {
                        'process_id': process.process_id,
                        'state': process.state,
                        'date': convert_date(process.date),
                        'time': process.time
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
