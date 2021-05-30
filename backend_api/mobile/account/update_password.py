"""
:author chautruonglong
"""

from django.shortcuts import render
from django.utils import timezone
from rest_framework.response import Response
from rest_framework.decorators import api_view, permission_classes
from rest_framework import status
from rest_framework_api_key.models import APIKey
from rest_framework_api_key.permissions import HasAPIKey
from hashlib import md5
from core.models import Account


@api_view(['PUT'])
@permission_classes((HasAPIKey, ))
def update_password_api(request):
    try:
        body = request.data
        email = body['email']
        old_pwd = body['old_pwd']
        new_pwd = body['new_pwd']

        account = Account.objects.get(email=email)

        if account.hash_pwd != old_pwd:
            raise Exception('Old password is not true')

        account.hash_pwd = new_pwd
        account.save()

        return Response(
            data={
                "message": "Update successful"
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
