from django.db import models
from rest_framework import serializers
from .models import JoinTable

class JoinTableSerializer(serializers.ModelSerializer):

    class Meta:
        model=JoinTable
        fields=['id_lecturer','date']