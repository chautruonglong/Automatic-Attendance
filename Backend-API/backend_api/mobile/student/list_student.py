from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Student, StudentSubject
from mobile.student.utils import StudentParser


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def list_student_api(request, subject_id):
    try:
        stu_subs = StudentSubject.objects.filter(subject_id=subject_id)

        json_data = []
        for stu_sub in stu_subs:
            student_id = stu_sub.student_id
            student = Student.objects.get(student_id=student_id)
            json_data.append(StudentParser(request, student).to_dict())

        return Response(
            data=json_data,
            status=status.HTTP_202_ACCEPTED
        )

    except Exception as error:
        print(str(error))
        return Response(
            data={
                "error_message": str(error)
            },
            status=status.HTTP_406_NOT_ACCEPTABLE
        )
