from django.db import models


# Create your models here.
class Account(models.Model):
    _id = models.CharField(max_length=100)
    password = models.CharField(max_length=100)
    level = models.PositiveSmallIntegerField(default=4)
    group = models.CharField(max_length=100)

