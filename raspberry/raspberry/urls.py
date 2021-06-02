from django.contrib import admin
from django.urls import path
from camera.api import open_camera
from camera.api import capture_camera
from camera.api import close_camera

urlpatterns = [
    path('', admin.site.urls),
    path('raspberry/camera/open/<int:video>/', open_camera.open_camera_api),
    path('raspberry/camera/capture/', capture_camera.capture_camera_api),
    path('raspberry/camera/close/', close_camera.close_camera_api),
]
