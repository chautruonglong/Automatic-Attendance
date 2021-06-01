"""
:author chautruonglong
"""

from django.shortcuts import render
from django.utils.http import urlsafe_base64_decode
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response

from backend_api.utils import mail_system
from backend_api.utils import token_generator
from core.models import Account, Lecturer


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

                token = token_generator.make_token(account)
                mail_system.send(request, email, token)

                return Response(
                    data={
                        "message": "Final step, A link has sent to your email"
                    },
                    status=status.HTTP_202_ACCEPTED
                )

        # Send first email
        lecturer = Lecturer.objects.create(name=name, phone=phone, faculty=faculty)
        account = Account.objects.create(email=email, hash_pwd=hash_pwd, lecturer=lecturer)
        token = token_generator.make_token(account)
        mail_system.send(request, email, token)

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

        if account.is_active or token_generator.check_token(account, token) is False:
            context = {
                'message': 'The account has been activated or the link is not true'
            }
            return render(request, 'wrong_link.html', context)
        else:
            account.is_active = True
            account.save()
            context = {
                'email': email
            }
            return render(request, 'activated_successfully.html', context)

    except Exception as error:
        context = {
            'message': str(error)
        }
        return render(request, 'wrong_link.html', context)
