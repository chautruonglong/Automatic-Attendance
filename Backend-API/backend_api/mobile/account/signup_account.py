"""
:author chautruonglong
"""

from django.shortcuts import render
from django.utils.http import urlsafe_base64_decode
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response

from backend_api.utils import mail_system
from backend_api.utils import activate_token
from core.models import Account, Lecturer
from backend_api.utils import get_base_url


@api_view(['POST'])
def signup_api(request):
    try:
        body = request.data
        name = body['name']
        phone = body['phone']
        faculty = body['faculty']
        email = body['email']
        hash_pwd = body['password']

        if not mail_system.verify(email):
            raise Exception('Email does not exists')

        account = Account.objects.filter(email=email)
        if account.exists():
            if account[0].is_active:
                raise Exception('Account already exists')

            else:
                # Resend email
                account = Account.objects.get(email=email)
                account.hash_pwd = hash_pwd
                account.save()

                Lecturer.objects.update_or_create(
                    lecturer_id=account.lecturer_id,
                    defaults={
                        'name': name,
                        'phone': phone,
                        'faculty': faculty
                    }
                )

                token = activate_token.make_token(account)
                mail_system.send_activate(request, email, token)

                return Response(
                    data={
                        "message": "Final step, A link has sent to your email"
                    },
                    status=status.HTTP_202_ACCEPTED
                )

        # Send first email
        lecturer = Lecturer.objects.create(name=name, phone=phone, faculty=faculty)
        account = Account.objects.create(email=email, hash_pwd=hash_pwd, lecturer=lecturer)
        token = activate_token.make_token(account)
        mail_system.send_activate(request, email, token)

        return Response(
            data={
                "message": "Final step, A link has sent to your email"
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


@api_view(['GET'])
def active_account_api(request, email_b64, token):
    try:
        email = urlsafe_base64_decode(email_b64).decode()
        account = Account.objects.get(email=email)

        if account.is_active or activate_token.check_token(account, token) is False:
            raise Exception('The account has been activated or the link is not true')

        else:
            account.is_active = True
            account.save()
            context = {
                'icon': f'{get_base_url(request)}/resources/icons/yes.jpg',
                'messages': [
                    f'Hi, your account: {email}',
                    f'You have successfully activated your account'
                ]
            }
            return render(request, 'successfully.html', context)

    except Exception as error:
        context = {
            'icon': f'{get_base_url(request)}/resources/icons/no.jpg',
            'messages': [
                'The link is not available',
                f'Error: {str(error)}'
            ]
        }
        return render(request, 'failure.html', context)
