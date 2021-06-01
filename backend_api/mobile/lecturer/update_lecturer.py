"""
:author chautruonglong
"""

from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Lecturer


@api_view(['PUT'])
@permission_classes((HasAPIKey, ))
def update_lecturer_api(request):
    try:
        body = request.data
        lecturer_id = body['lecturer_id']
        name = body['name']
        phone = body['phone']
        faculty = body['faculty']

        lecturer = Lecturer.objects.get(lecturer_id=lecturer_id)
        lecturer.name = name
        lecturer.phone = phone
        lecturer.faculty = faculty
        lecturer.save()

        return Response(
            data={
                'message': 'Updated'
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
