from django.shortcuts import render
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from .serializers import ProcessSerializer
from .models import Process
from rest_framework import permissions
from .permissions import IsOwner


class ProcessListAPIView(ListCreateAPIView):
    serializer_class = ProcessSerializer
    queryset = Process.objects.all()
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
        return serializer.save(owner=self.request.user)

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)


class ProcessDetailAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = ProcessSerializer
    permission_classes = (permissions.IsAuthenticated, IsOwner,)
    queryset = Process.objects.all()
    lookup_field = "id_process"

    def get_queryset(self):
        return self.queryset.filter(owner=self.request.user)





































# from django.http import request
# from django.shortcuts import render
# from .serializers import LecturerSerializer
# from .models import Lecturer
# # Create your views here.
# from rest_framework.generics import CreateAPIView
# from rest_framework import response, serializers, status,generics

# from rest_framework.response import Response
# from rest_framework.decorators import api_view

# class LecturerList(generics.ListAPIView):

#     serializer_class = LecturerSerializer

#     def get(slef, request):
#         try:
#             lookup_field = "id_lecturer"
#             lecturer = Lecturer.objects.get(lookup_field)
#         except Lecturer.DoesNotExist:
#             return Response(status=status.HTTP_404_NOT_FOUND)

#         if request.method == "GET":
#             serializer = LecturerSerializer(lecturer)
#         return Response(serializer.data)

# class LecturerCreate(generics.CreateAPIView):
    
#     serializer_class = LecturerSerializer

#     def post(self, request):
#         id_lecturer = request.data
#         serializer = self.serializer_class(data=id_lecturer)
#         serializer.is_valid(raise_exception=True)
#         serializer.save()

#         return Response(id_lecturer, status=status.HTTP_201_CREATED)
