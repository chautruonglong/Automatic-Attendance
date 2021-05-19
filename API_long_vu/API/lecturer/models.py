from django.db import models
from account.models import Account


# Create your models here.


class Lecturer(models.Model):

    #id_lecturer = models.ForeignKey(Account, on_delete=models.CASCADE)
    id_lecturer  = models.CharField(max_length=30,primary_key=True)

    name         = models.CharField(max_length=30,null=False)
    age          = models.IntegerField(null=False)
    phone        = models.IntegerField(null=False)

    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)










# from django.db import models

# from django.utils.text import slugify
# from django.conf import settings
# from django.db.models.signals import post_delete, pre_save
# from django.dispatch import receiver
# from account.models import Account

# # Create your models here.
# class Lecturer(models.Model):

#     id_lecturer = models.ForeignKey(Account, on_delete=models.CASCADE)
#     #id_lecturer  = models.CharField(max_length=30,primary_key = True)

#     name         = models.CharField(max_length=30,null=False)
#     age          = models.IntegerField(null=False)
#     phone        = models.IntegerField(null=False)

#     def __str__(self):  # show the id_lecturer of lecturer when print
#         return self.id_lecturer

