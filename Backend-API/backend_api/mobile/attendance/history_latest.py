from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey
from django.db.models import Max
from core.models import Process


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def history_latest_api(request, subject_id, date):
    try:
        processes = Process.objects.filter(subject_id=subject_id, date=date)
        process = processes.order_by('-time').first()

        return Response(
            data=process.process_id,
            status=status.HTTP_202_ACCEPTED
        )

    except Exception as error:
        return Response(
            data={
                'error_message': str(error)
            },
            status=status.HTTP_406_NOT_ACCEPTABLE
        )
