from django.http import request
from django.shortcuts import render
from .serializers import RoomSerializer
from .models import Room
from rest_framework import permissions
# Create your views here.
from rest_framework.generics import ListCreateAPIView, RetrieveUpdateDestroyAPIView
from rest_framework import permissions, response, status

from rest_framework.response import Response
from rest_framework.decorators import api_view


class RoomAPIView(RetrieveUpdateDestroyAPIView):
    serializer_class = RoomSerializer
    queryset = Room.objects.all()

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
            rooms = Room.objects.get(pk=pk)
        except Room.DoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        if request.method == 'GET':
            rooms = Room.objects.all()
            serializer = RoomSerializer(rooms, many=True)
            return Response(serializer.data)

        elif request.method == 'POST':
            serializer = RoomSerializer(data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data, status=status.HTTP_201_CREATED)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'PUT':
            serializer = RoomSerializer(rooms, data=request.data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data)
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

        elif request.method == 'DELETE':
            rooms.delete()
            return Response(status=status.HTTP_204_NO_CONTENT)