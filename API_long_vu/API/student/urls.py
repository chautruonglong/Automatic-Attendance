from django.urls import path
from . import views


urlpatterns = [
    path('', views.StudentListAPIView.as_view(), name="student"),
    path('<int:id>', views.StudentDetailAPIView.as_view(), name="student"),
]