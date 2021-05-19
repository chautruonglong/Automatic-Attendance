from django.urls import path
from rest_framework.decorators import api_view
from .views import ProcessListAPIView,ProcessDetailAPIView


urlpatterns = [
    #path('', LecturerCreateAPIView.as_view(), name="lecturer"), 
    path('', ProcessListAPIView.as_view(), name="Process"),
    path('<int:id_lecturer>', ProcessDetailAPIView.as_view(), name="Process"),

]

