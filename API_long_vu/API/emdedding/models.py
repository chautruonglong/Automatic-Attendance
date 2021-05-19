from django.db import models
from datetime import datetime
# from schedule.models import Schedule
from student.models import Student

# Create your models here.
class Emdedding(models.Model):

    id_emdedding    = models.IntegerField(default=100000000, primary_key = True)
    id_student      = models.ForeignKey(Student, on_delete=models.CASCADE)
    link            = models.CharField(max_length=255)