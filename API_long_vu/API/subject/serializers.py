from django.db import models
from rest_framework import serializers
from .models import Subject

class SubjectSerializer(serializers.ModelSerializer):

    class Meta:
        model=Subject
        fields=['id_subject','id_lecturer','id_room','nameSubject','semester']