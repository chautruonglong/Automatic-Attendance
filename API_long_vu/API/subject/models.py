from django.db import models

# Create your models here.
class Subject(models.Model):

    id_subject  = models.IntegerField(default=100000000, primary_key = True)
    id_lecturer = models.IntegerField(max_length=30,null=False,default=100000000)
    id_room     = models.CharField(max_length=30,null=False,default=100000000)
    name        = models.CharField(max_length=30,null=False)
    date        = models.DateField(null=False)
    time        = models.TimeField(null=False)