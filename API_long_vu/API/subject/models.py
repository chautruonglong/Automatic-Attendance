from django.db import models
from lecturer.models import Lecturer
from room.models import Room
from account.models import Account
# Create your models here.
class Subject(models.Model):

    id_subject = models.CharField(max_length=30, primary_key = True)
    id_lecturer = models.ForeignKey(Lecturer, on_delete=models.CASCADE)
    id_room = models.ForeignKey(Room, on_delete=models.CASCADE)
    nameSubject = models.CharField(max_length=30)
    semester = models.CharField(max_length=30)
    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)