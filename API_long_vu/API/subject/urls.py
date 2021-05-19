from django.urls import path
from rest_framework.decorators import api_view
from .views import SubjectListAPIView,SubjectDetailAPIView


urlpatterns = [
    path('', SubjectListAPIView.as_view(), name="lecturer"),
    path('<int:id_subject>', SubjectDetailAPIView.as_view(), name="lecturer"),

]

