from django.urls import path
from . import views

urlpatterns = [
    path('', views.StudentAPIView.as_view(), name="student"),
]
