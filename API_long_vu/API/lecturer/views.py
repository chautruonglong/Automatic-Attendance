from django.http import request
from django.shortcuts import render
from .serializers import LecturerSerializer
from .models import Lecturer
# Create your views here.
from rest_framework.generics import RetrieveUpdateDestroyAPIView
from rest_framework import response, status

from rest_framework.response import Response
from rest_framework.decorators import api_view


class LecturerAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = LecturerSerializer
    queryset = Lecturer.objects.all()

    def room_create(self, serializer):
        return serializer.save(owner=self.request.user)
    
    def get_queryset(self):
        return self.queryset.fillter(owner=self.request.user)

    def post(self, request):
            serializer = self.serializer_class(data= request.data)
            if serializer.is_valid():
                serializer.save()
                return response.Response(serializer.data, status=status.HTTP_201_CREATED)
            return response.Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    @api_view(['GET', 'POST','PUT', 'DELETE'])
    def student_list(request,pk):
        try:
            lecturer = Lecturer.objects.get(pk=pk)
        except Lecturer.DoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        if request.method == 'GET':
            lecturer = Lecturer.objects.all()
            serializer = LecturerSerializer(lecturer, many=True)
            return Response(serializer.data)

        elif request.method == 'POST':
            serializer = LecturerSerializer(data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data, status=status.HTTP_201_CREATED)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'PUT':
            serializer = LecturerSerializer(lecturer, data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'DELETE':
            lecturer.delete()
            return Response(status=status.HTTP_204_NO_CONTENT)

















# from rest_framework import permissions, response, status

# from rest_framework.response import Response
# from rest_framework.decorators import api_view
# from .models import Student
# from .serializers import subjectserializer

# # Create your views here.
# from rest_framework.generics import GenericAPIView

# from student import serializers


# class StudentAPIView(GenericAPIView):

#     serializer_class = subjectserializer

#     def post(self, request):
#         serializer = self.serializer_class(data= request.data)
            
#         if serializer.is_valid():
#             serializer.save()
#             return response.Response(serializer.data, status=status.HTTP_201_CREATED)

#         return response.Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

#     @api_view(['GET'])
#     def get(self, request):
#         try:
#             student = Student.objects.all()
#         except Student.DoesNotExist:
#             return Response(status=status.HTTP_404_NOT_FOUND)

#         if request.method == 'GET':
#             serializers = subjectserializer(student)
#             return Response(serializers.data)

