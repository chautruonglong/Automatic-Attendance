"""
:author chautruonglong
"""

from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Lecturer


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def detail_lecturer_api(request, lecturer_id):
    try:
        lecturer = Lecturer.objects.get(lecturer_id=lecturer_id)
        return Response(
            data={
                'name': lecturer.name,
                'phone': lecturer.phone,
                'faculty': lecturer.faculty
            },
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
