from django.urls import path
from . import views

urlpatterns = [
    path('', views.RoomListAPIView.as_view(), name="room"),
    path('<int:id>', views.RoomDetailAPIView.as_view(), name="room"),
]