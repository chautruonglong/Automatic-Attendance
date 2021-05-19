from django.db import models
from rest_framework import serializers
from .models import Process

class ProcessSerializer(serializers.ModelSerializer):

    class Meta:
        model=Process
        fields=['id_process','id_subject','status','date','timeSlot']