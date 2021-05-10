from django.db import models

# Create your models here.
class Student(models.Model):

    id_student = models.IntegerField(null=False, default=100000000, primary_key = True)
    name       = models.CharField(max_length=30,null=False)
    phone      = models.IntegerField(null=False, default=0)
    age        = models.IntegerField(null=True, default=0)
    image      = models.ImageField(null=True, blank = True)

