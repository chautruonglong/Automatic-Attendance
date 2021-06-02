"""
:author chautruonglong
"""

from django.utils import timezone
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework_api_key.models import APIKey

from core.models import Account


@api_view(['POST'])
def login_api(request):
    try:
        body = request.data
        email = body['email']
        password = body['password']
        hash_pwd = password

        account = Account.objects.get(email=email)

        if account.is_active is False:
            raise Exception('Your account is not activated')
        if hash_pwd != account.hash_pwd:
            raise Exception('Wrong password')

        APIKey.objects.filter(name=email).delete()
        expiry_date = timezone.localtime() + timezone.timedelta(hours=24)
        user, key = APIKey.objects.create_key(name=email, expiry_date=expiry_date)

        return Response(
            data={
                'lecturer_id': account.lecturer_id,
                'authorization': f'Api-Key {key}'
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
