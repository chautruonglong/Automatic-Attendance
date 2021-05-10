from django.urls import path
from . import views

urlpatterns = [
    path('', views.SubjectAPIView.as_view(), name="subject"),
]
