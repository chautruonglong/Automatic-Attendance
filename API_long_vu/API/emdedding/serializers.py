from django.db import models
from rest_framework import serializers
from .models import Emdedding

class EmdeddingSerializer(serializers.ModelSerializer):

    class Meta:
        model=Emdedding
        fields=['id_emdedding','id_student','link']