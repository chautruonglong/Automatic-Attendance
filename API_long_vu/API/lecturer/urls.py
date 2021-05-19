from django.urls import path
from rest_framework.decorators import api_view
from .views import LecturerListAPIView,LecturerDetailAPIView


urlpatterns = [
    #path('', LecturerCreateAPIView.as_view(), name="lecturer"), 
    path('', LecturerListAPIView.as_view(), name="lecturer"),
    path('<int:id_lecturer>', LecturerDetailAPIView.as_view(), name="lecturer"),

]

