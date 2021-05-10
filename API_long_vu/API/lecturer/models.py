from django.db import models

# Create your models here.
class Lecturer(models.Model):

    id_lecturer  = models.IntegerField(default=100000000, primary_key = True)
    name         = models.CharField(max_length=30,null=False)
    phone        = models.IntegerField(null=False)
    age          = models.IntegerField(null=False)
