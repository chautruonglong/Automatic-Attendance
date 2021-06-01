from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Attendance, StudentSubject


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def detail_history_api(request, process_id):
    try:
        attendances = Attendance.objects.filter(process_id=process_id)

        data_json = []
        for attendance in attendances:
            stusub = StudentSubject.objects.get(stusub_id=attendance.stusub_id)
            student_id = stusub.student_id
            data_json.append(
                {
                    'student_id': student_id
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
