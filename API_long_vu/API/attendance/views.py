from django.shortcuts import render
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from .serializers import AttendanceSerializer
from .models import  Attendance
from rest_framework import permissions
from .permissions import IsOwner


class AttendanceListAPIView(ListCreateAPIView):
    serializer_class = AttendanceSerializer
    queryset = Attendance.objects.all()
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
        return serializer.save(owner=self.request.user)

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)


class AttendanceDetailAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = AttendanceSerializer
    permission_classes = (permissions.IsAuthenticated, IsOwner,)
    queryset = Attendance.objects.all()
    lookup_field = "id_attendance"

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)