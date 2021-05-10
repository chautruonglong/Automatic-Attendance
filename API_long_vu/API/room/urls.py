from django.urls import path
from . import views

urlpatterns = [
    path('', views.RoomAPIView.as_view(), name="room"),
    
]
