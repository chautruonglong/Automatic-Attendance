from django.shortcuts import render
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from .serializers import RoomSerializer
from .models import  Room
from rest_framework import permissions
from .permissions import IsOwner


class RoomListAPIView(ListCreateAPIView):
    serializer_class = RoomSerializer
    queryset = Room.objects.all()
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
        return serializer.save(owner=self.request.user)

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)


class RoomDetailAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = RoomSerializer
    permission_classes = (permissions.IsAuthenticated, IsOwner,)
    queryset = Room.objects.all()
    lookup_field = "id_Room"

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)