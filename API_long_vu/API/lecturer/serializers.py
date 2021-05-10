from django.db import models
from rest_framework import serializers
from .models import Lecturer

class LecturerSerializer(serializers.ModelSerializer):

    class Meta:
        model=Lecturer
        fields=['id_lecturer','name','phone','age']