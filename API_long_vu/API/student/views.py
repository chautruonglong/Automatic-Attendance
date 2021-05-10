from django.http import request
from django.shortcuts import render
from .serializers import StudentSerializer
from .models import Student
from rest_framework import permissions
# Create your views here.
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from rest_framework import permissions, response, status

from rest_framework.response import Response
from rest_framework.decorators import api_view


class StudentAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = StudentSerializer
    queryset = Student.objects.all()

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
            students = Student.objects.get(pk=pk)
        except Student.DoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        if request.method == 'GET':
            students = Student.objects.all()
            serializer = StudentSerializer(students, many=True)
            return Response(serializer.data)

        elif request.method == 'POST':
            serializer = StudentSerializer(data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data, status=status.HTTP_201_CREATED)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'PUT':
            serializer = StudentSerializer(students, data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'DELETE':
            students.delete()
            return Response(status=status.HTTP_204_NO_CONTENT)

















# from rest_framework import permissions, response, status

# from rest_framework.response import Response
# from rest_framework.decorators import api_view
# from .models import Student
# from .serializers import StudentSerializer

# # Create your views here.
# from rest_framework.generics import GenericAPIView

# from student import serializers


# class StudentAPIView(GenericAPIView):

#     serializer_class = StudentSerializer

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
#             serializers = StudentSerializer(student)
#             return Response(serializers.data)

