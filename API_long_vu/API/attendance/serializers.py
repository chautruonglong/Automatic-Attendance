from django.db import models
from rest_framework import serializers
from .models import Attendance

class AttendanceSerializer(serializers.ModelSerializer):

    class Meta:
        model=Attendance
        fields=['id_attendance','id_student','id_schedule','absent','time','face_img']