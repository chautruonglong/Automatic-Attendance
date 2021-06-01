from django import forms
from django.shortcuts import render
from rest_framework.decorators import api_view


class ResetForm(forms.Form):
    new_password = forms.CharField(label='new_password', max_length=1023)
    re_password = forms.CharField(label='re_password', max_length=1023)


@api_view(['GET'])
def reset_password_api(request):
    return render(request, 'reset_password.html')
