"""
:author chautruonglong
"""

from datetime import datetime
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from random import randint
from smtplib import SMTP_SSL
from threading import Thread

from django.contrib.auth.tokens import PasswordResetTokenGenerator
from django.shortcuts import render
from django.utils import six
from django.utils.crypto import salted_hmac
from django.utils.http import int_to_base36
from django.utils.http import urlsafe_base64_encode
from validate_email import validate_email

from backend_api import settings


class ActivateToken(PasswordResetTokenGenerator):
    def _make_hash_value(self, account, timestamp):
        return (
                six.text_type(account.email) + six.text_type(timestamp) +
                six.text_type(account.is_active)
        )

    def _make_token_with_timestamp(self, account, timestamp):
        ts_b36 = int_to_base36(timestamp)
        hash = salted_hmac(
            self.key_salt,
            self._make_hash_value(account, timestamp),
        ).hexdigest()
        return f'{ts_b36}-{hash}'


class MailSystem:
    def __init__(self):
        self._mail_server = SMTP_SSL(
            settings.EMAIL_HOST,
            settings.EMAIL_PORT
        )

        self._mail_server.ehlo()

        self._mail_server.login(
            settings.EMAIL_HOST_USER,
            settings.EMAIL_HOST_PASSWORD
        )

    def verify(self, email):
        is_valid = validate_email(email, check_mx=True, verify=True)
        return is_valid is True

    def _run(self, request, email, token):
        email_b64 = urlsafe_base64_encode(email.encode())
        base_url = get_base_url(request)
        link = f'{base_url}/account/signup/activate/{email_b64}/{token}'
        context = {
            'link': link
        }
        body = render(request, 'email_body.html', context).content.decode('utf-8')

        message = MIMEMultipart('The email')
        message['Subject'] = 'Email Verification'
        message['From'] = settings.EMAIL_HOST_USER
        message['To'] = email
        message.attach(MIMEText(body, 'html'))

        self._mail_server.sendmail(
            settings.EMAIL_HOST_USER,
            email,
            message.as_string()
        )

    def send(self, request, email, token):
        thread = Thread(target=self._run, args=(request, email, token))
        print(f'Send email to {email}')
        thread.start()


def get_base_url(request):
    return request.build_absolute_uri().replace(request.get_full_path(), '')


def convert_date(old_date):
    old_format = '%Y-%m-%d'
    new_format = '%d-%m-%Y'
    return datetime.strptime(str(old_date), old_format).strftime(new_format)


def convert_date_django(old_date):
    old_format = '%d-%m-%Y'
    new_format = '%Y-%m-%d'
    return datetime.strptime(str(old_date), old_format).strftime(new_format)


def convert_time(old_time):
    old_time = str(old_time)
    old_time = old_time[:old_time.index('.')]
    old_format = '%H:%M:%S'
    new_format = '%H-%M-%S'
    return datetime.strptime(old_time, old_format).strftime(new_format)


def generate_date():
    return datetime.strptime(f'{randint(1, 28):02d}-{randint(1, 12):02d}-2000', '%d-%m-%Y')


def generate_process_id(subject_id, date, time, size=255):
    KEY_SALT = 'automatic.attendance.process.id'
    value = six.text_type(subject_id) + six.text_type(date) + six.text_type(time)
    hash = salted_hmac(KEY_SALT, value)
    return hash.hexdigest()[:size]


token_generator = ActivateToken()
mail_system = MailSystem()
