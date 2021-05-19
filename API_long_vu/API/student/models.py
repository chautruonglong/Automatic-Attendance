from django.db import models
from account.models import Account
# Create your models here.
class Student(models.Model):

    #id_student = models.IntegerField(null=False, default=100000000, primary_key = True)

    id_student = models.IntegerField(default=100000000, primary_key = True)
    name       = models.CharField(max_length=30,null=False)
    phone      = models.IntegerField(null=False, default=0)
    age        = models.IntegerField(null=True, default=0)
    birthday   = models.DateField(auto_now_add=True)
    image      = models.ImageField(null=True, blank = True)

    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)


    def __str__(self):
        return str(self.owner)