from django.db import models

# Create your models here.
from django.contrib.auth.models import (
    AbstractBaseUser, BaseUserManager, PermissionsMixin)

from django.db import models
from rest_framework_simplejwt.tokens import RefreshToken


class AccountManager(BaseUserManager):

    def create_user(self, username, email, password=None):
        if username is None:
            raise TypeError('Users should have a username')
        if email is None:
            raise TypeError('Users should have a Email')

        user = self.model(username=username, email=self.normalize_email(email))
        user.set_password(password)
        user.save()
        return user

    def create_superuser(self, username, email, password=None):
        if password is None:
            raise TypeError('Password should not be none')

        user = self.create_user(username, email, password)
        user.is_superuser = True
        user.is_staff = True
        user.save()
        return user


AUTH_PROVIDERS = { 'email': 'email'}


class Account(AbstractBaseUser, PermissionsMixin):
    username = models.CharField(max_length=255, unique=True, db_index=True)
    email = models.EmailField(max_length=255, unique=True, db_index=True)
    is_verified = models.BooleanField(default=False)
    is_active = models.BooleanField(default=True)
    is_staff = models.BooleanField(default=False)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
    auth_provider = models.CharField(
        max_length=255, blank=False,
        null=False, default=AUTH_PROVIDERS.get('email'))

    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = ['username']

    objects = AccountManager()

    def __str__(self):
        return self.email

    def tokens(self):
        refresh = RefreshToken.for_user(self)
        return {
            'refresh': str(refresh),
            'access': str(refresh.access_token)
        }


# class Account(AbstractBaseUser):
    
#     # id_lecturer = models.ForeignKey(lecturer.models.Lecturer, on_delete=models.CASCADE)
#     # email = models.EmailField(max_length=255, unique=True, db_index=True, primary_key=True)
    
#     id_lecturer  = models.CharField(max_length=30,primary_key = True)
#     email = models.EmailField(max_length=255, unique=True, db_index=True)

#     is_verified = models.BooleanField(default=False)
#     is_active = models.BooleanField(default=True)
#     is_staff = models.BooleanField(default=False)
#     created_at = models.DateTimeField(auto_now_add=True)
#     updated_at = models.DateTimeField(auto_now=True)
#     auth_provider = models.CharField(
#         max_length=255, blank=False,
#         null=False, default=AUTH_PROVIDERS.get('email'))

#     USERNAME_FIELD = 'email'
#     REQUIRED_FIELDS = ['id_lecturer']

#     # objects = AccountManager()

#     def __str__(self):
#         return self.email

#     def tokens(self):
#         refresh = RefreshToken.for_user(self)
#         return {
#             'refresh': str(refresh),
#             'access': str(refresh.access_token)
#         }




# from django.db import models
# import lecturer.models
# from rest_framework_simplejwt.tokens import RefreshToken


# class Account(models.Model):
#     id_lecturer = models.ForeignKey(Lecturer, on_delete=models.CASCADE)
#     email = models.EmailField(max_length=255, unique=True, db_index=True, primary_key=True)

#     def __str__(self):
#         return self.email

#     def tokens(self):
#         refresh = RefreshToken.for_user(self)
#         return {
#             'refresh': str(refresh),
#             'access': str(refresh.access_token)
#         }