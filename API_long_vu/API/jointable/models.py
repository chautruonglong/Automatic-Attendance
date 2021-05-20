from django.db import models
from subject.models import Subject
from account.models import Account
from process.models import Process
# Create your models here.
class JoinTable(models.Model):

    id_process = models.ForeignKey(Process, on_delete=models.CASCADE)
    id_subject = models.ForeignKey(Subject, on_delete=models.CASCADE)

    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)