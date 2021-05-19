from django.db import models
from rest_framework import serializers
from .models import Room

class RoomSerializer(serializers.ModelSerializer):

    class Meta:
        model=Room
        fields=['id_room','state','name']