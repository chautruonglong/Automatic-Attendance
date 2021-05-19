from django.db import models
from datetime import datetime
# from schedule.models import Schedule
from student.models import Student
from account.models import Account
now = datetime.now()
# Create your models here.
class Attendance(models.Model):

    id_attendance   = models.IntegerField(default=100000000, primary_key = True)
    id_student      = models.ForeignKey(Student, on_delete=models.CASCADE)
    # id_schedule     = models.ForeignKey(Schedule, on_delete=models.CASCADE)

    absent          = models.CharField(max_length=30,null=False)
    time            = models.TimeField(default=now)
    face_img        = models.ImageField(null = True)

    owner = models.ForeignKey(to=Account, on_delete=models.CASCADE)
    # date = models.DateField(null=False, blank=False)
    # created_at = models.DateTimeField(auto_now_add=True)
    # updated_at = models.DateTimeField(auto_now=True)

    # class Meta:
    #     ordering: ['-updated_at']

    def __str__(self):
        return str(self.owner)