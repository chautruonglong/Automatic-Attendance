from django import forms
from rest_framework.response import Response
from django.shortcuts import render
from rest_framework.decorators import api_view
from rest_framework import status
from backend_api.utils import reset_token, get_base_url
from backend_api.utils import mail_system
from django.utils.http import urlsafe_base64_decode
from core.models import Account
from hashlib import md5


@api_view(['GET'])
def reset_password_api(request, email):
    try:
        account = Account.objects.get(email=email)

        token = reset_token.make_token(account)
        mail_system.send_reset(request, email, token)

        return Response(
            data={
                'message': 'Check your email to change password'
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


class ResetForm(forms.Form):
    new_password = forms.CharField(label='new_password', max_length=1023)
    re_password = forms.CharField(label='re_password', max_length=1023)


@api_view(['GET', 'POST'])
def reset_password_form_api(request, email_b64, token):
    try:
        email = urlsafe_base64_decode(email_b64).decode()
        account = Account.objects.get(email=email)

        if reset_token.check_token(account, token) is False:
            raise Exception('The link has expired')

        if account.is_active is False:
            raise Exception('The account is not active')

        if request.method == 'GET':
            context = {
                'email': email
            }
            return render(request, 'reset_password.html', context=context)

        elif request.method == 'POST':
            form = ResetForm(request.POST)
            if form.is_valid():
                new_pwd = form.cleaned_data['new_password']
                re_pwd = form.cleaned_data['re_password']

                if new_pwd != re_pwd:
                    raise Exception('Passwords are not the same')

                if account.hash_pwd == new_pwd:
                    raise Exception('Cannot be the same as the old password')

                account.hash_pwd = md5(new_pwd.encode()).hexdigest()
                account.save()

                context = {
                    'icon': f'{get_base_url(request)}/resources/icons/yes.jpg',
                    'messages': [
                        'Change password successfully'
                    ]
                }
                return render(request, 'successfully.html', context=context)

    except Exception as error:
        print(str(error))
        context = {
            'icon': f'{get_base_url(request)}/resources/icons/no.jpg',
            'messages': [
                f'Error: {str(error)}'
            ]
        }
        return render(request, 'failure.html', context)




