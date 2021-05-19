from django.db import models
from subject.models import Subject
from account.models import Account
# Create your models here.
class Process(models.Model):

    id_process = models.IntegerField(default=100000000, primary_key = True)
    id_subject = models.ForeignKey(Subject, on_delete=models.CASCADE)
    status = models.BooleanField()
    date = models.DateField(auto_now_add=True)
    timeSlot = models.TimeField(auto_now=True)
    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)