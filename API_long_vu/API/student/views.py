from django.shortcuts import render
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from .serializers import StudentSerializer
from .models import  Student
from rest_framework import permissions
from .permissions import IsOwner


class StudentListAPIView(ListCreateAPIView):
    serializer_class = StudentSerializer
    queryset = Student.objects.all()
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
    		
        return serializer.save(owner=self.request.user)

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)


class StudentDetailAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = StudentSerializer
    permission_classes = (permissions.IsAuthenticated, IsOwner,)
    queryset = Student.objects.all()
    lookup_field = "id_student"

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)