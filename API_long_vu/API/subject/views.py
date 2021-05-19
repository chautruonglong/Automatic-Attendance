from django.shortcuts import render
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from .serializers import SubjectSerializer
from .models import Subject
from rest_framework import permissions
from .permissions import IsOwner


class SubjectListAPIView(ListCreateAPIView):
    serializer_class = SubjectSerializer
    queryset = Subject.objects.all()
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
        return serializer.save(owner=self.request.user)

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)


class SubjectDetailAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = SubjectSerializer
    permission_classes = (permissions.IsAuthenticated, IsOwner,)
    queryset = Subject.objects.all()
    lookup_field = "id_subject"

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)



