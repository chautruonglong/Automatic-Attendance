from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey
from core.models import StudentSubject, Attendance, Subject


@api_view(['PUT'])
@permission_classes((HasAPIKey, ))
def update_attendance_api(request, subject_id, process_id):
    try:
        students = request.data

        for student in students:
            stusub_id = StudentSubject.objects.get(student_id=student['student_id'], subject_id=subject_id).stusub_id
            if student['state'] is False:
                Attendance.objects.filter(stusub_id=stusub_id, process_id=process_id).delete()
            elif not Attendance.objects.filter(stusub_id=stusub_id, process_id=process_id).exists():
                Attendance.objects.create(
                    stusub_id=stusub_id,
                    process_id=process_id,
                    img_face=None,
                    confidence=None
                )

        return Response(
            data={
                'message': 'Updated successfully'
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
