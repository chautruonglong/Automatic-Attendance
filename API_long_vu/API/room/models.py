from django.db import models
from account.models import Account
# Create your models here.
class Room(models.Model):

    id_room = models.IntegerField(default=100000000, primary_key = True)
    state = models.CharField(max_length=30,default='A')
    name = models.CharField(max_length=30,null=False)

    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)