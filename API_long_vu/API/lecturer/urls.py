from django.urls import path
from . import views

urlpatterns = [
    path('', views.LecturerAPIView.as_view(), name="subject"),
]
