from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework import status
from rest_framework_api_key.permissions import HasAPIKey
from datetime import datetime
from backend_api.utils import generate_process_id
from core.models import Process
from recognition.attendance_process import attendance_process


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def create_process_api(request, subject_id):
    try:
        now = datetime.now()
        process_id = generate_process_id(subject_id, now.date(), now.time())
        Process.objects.create(
            process_id=process_id,
            subject_id=subject_id,
            state=False,
            date=now.date(),
            time=now.time()
        )

        attendance_process.attendance(process_id, subject_id, window=False)

        return Response(
            data={
                'process_id': process_id
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
