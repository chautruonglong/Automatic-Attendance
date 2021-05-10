from django.db import models

# Create your models here.
class Room(models.Model):

    id_room = models.IntegerField(null=False)
    zone = models.CharField(max_length=30,null=False)
    name = models.CharField(max_length=30,null=False)


