"""
:author chautruonglong
"""

from rest_framework import status
from rest_framework.decorators import api_view, permission_classes
from rest_framework.response import Response
from rest_framework_api_key.permissions import HasAPIKey

from core.models import Subject
from mobile.subject.utils import SubjectParser


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def list_subject_api(request, lecturer_id, day):
    try:
        if day == 'all':
            subjects = Subject.objects.filter(lecturer_id=lecturer_id)
        else:
            subjects = Subject.objects.filter(lecturer_id=lecturer_id, day=day)

        json_data = [SubjectParser(subject).to_dict() for subject in subjects]

        return Response(
            data=json_data,
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
