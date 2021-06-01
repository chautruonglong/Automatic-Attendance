from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response

from core.models import Attendance, StudentSubject, Process, Unknown
from mobile.attendance.utils import AttendanceParser


@api_view(['GET'])
# @permission_classes((HasAPIKey, ))
def list_attendance_api(request, process_id):
    try:
        process = Process.objects.get(process_id=process_id)
        if process.state is False:
            raise Exception('Attendance process is running')

        attendances = Attendance.objects.filter(process_id=process_id)
        unknowns = Unknown.objects.filter(process_id=process_id)

        data_json = []
        for attendance in attendances:
            stusub = StudentSubject.objects.get(stusub_id=attendance.stusub_id)
            student_id = stusub.student_id
            data_json.append(
                AttendanceParser(
                    request=request,
                    type='known',
                    id=student_id,
                    img_face=attendance.img_face,
                    confidence=attendance.confidence
                ).to_dict()
            )

        for unknown in unknowns:
            data_json.append(
                AttendanceParser(
                    request=request,
                    type='unknown',
                    id=unknown.unknown_id,
                    img_face=unknown.img_face,
                    confidence=unknown.confidence
                ).to_dict()
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
