"""
:author chautruonglong
"""

from django.db import models

from backend_api.utils import generate_date


class Student(models.Model):
    student_id = models.CharField(primary_key=True, max_length=255)
    name = models.CharField(null=True, max_length=1023)
    phone = models.CharField(null=True, max_length=255)
    birth = models.DateField(default=generate_date())
    class_name = models.CharField(null=True, max_length=255)
    img_3x4 = models.ImageField(null=True, upload_to='images/temps/')

    def __str__(self):
        return f'{str(self.name)} - {str(self.student_id)}'

    class Meta:
        verbose_name = 'Student'
        verbose_name_plural = 'Students'


class Lecturer(models.Model):
    lecturer_id = models.BigAutoField(primary_key=True)
    name = models.CharField(null=True, max_length=1023)
    phone = models.CharField(null=True, max_length=255)
    faculty = models.CharField(null=True, max_length=1023)

    def __str__(self):
        return f'{str(self.name)} - {str(self.lecturer_id)}'

    class Meta:
        verbose_name = 'Lecturer'
        verbose_name_plural = 'Lecturers'


class Account(models.Model):
    email = models.EmailField(primary_key=True, max_length=1023)
    hash_pwd = models.CharField(null=False, max_length=1023)
    is_active = models.BooleanField(default=False)
    lecturer = models.ForeignKey(to=Lecturer, on_delete=models.CASCADE, null=False)

    def __str__(self):
        return str(self.email)

    class Meta:
        verbose_name = 'Account'
        verbose_name_plural = 'Accounts'


class Room(models.Model):
    room_id = models.CharField(primary_key=True, max_length=255)
    zone = models.CharField(null=True, max_length=255)
    ip_cam = models.CharField(null=True, max_length=255)

    def __str__(self):
        return str(self.room_id)

    def save(self, *args, **kwargs):
        self.room_id = self.room_id.upper()
        self.zone = self.room_id[0]
        super(Room, self).save(*args, **kwargs)

    class Meta:
        verbose_name = 'Room'
        verbose_name_plural = 'Rooms'


class Subject(models.Model):
    subject_id = models.CharField(primary_key=True, max_length=255)
    lecturer = models.ForeignKey(to=Lecturer, on_delete=models.CASCADE, null=False)
    room = models.ForeignKey(to=Room, on_delete=models.CASCADE, null=False)
    name = models.CharField(null=True, max_length=1023)
    time_slot = models.CharField(null=True, max_length=255)
    day = models.CharField(null=True, max_length=255)

    def __str__(self):
        return f'{str(self.name)} - {str(self.subject_id)}'

    class Meta:
        verbose_name = 'Subject'
        verbose_name_plural = 'Subjects'


class StudentSubject(models.Model):
    stusub_id = models.BigAutoField(primary_key=True)
    subject = models.ForeignKey(to=Subject, on_delete=models.CASCADE, null=False)
    student = models.ForeignKey(to=Student, on_delete=models.CASCADE, null=False)

    def __str__(self):
        return str(self.stusub_id)

    class Meta:
        verbose_name = 'StudentSubject'
        verbose_name_plural = 'StudentSubjects'


class Process(models.Model):
    process_id = models.CharField(primary_key=True, max_length=255)
    subject = models.ForeignKey(to=Subject, on_delete=models.CASCADE, null=False)
    state = models.BooleanField(default=False)
    date = models.DateField(auto_now=True)
    time = models.TimeField(auto_now=True)

    def __str__(self):
        return str(self.process_id)

    class Meta:
        verbose_name = 'Process'
        verbose_name_plural = 'Processes'


class Attendance(models.Model):
    attendance_id = models.BigAutoField(primary_key=True)
    stusub = models.ForeignKey(to=StudentSubject, on_delete=models.CASCADE, null=False)
    process = models.ForeignKey(to=Process, on_delete=models.CASCADE, null=False)
    img_face = models.ImageField(null=True, upload_to='images/temps/', max_length=1023)
    confidence = models.FloatField(null=True, default=100)

    def __str__(self):
        return str(self.attendance_id)

    class Meta:
        verbose_name = 'Attendance'
        verbose_name_plural = 'Attendances'


class Unknown(models.Model):
    unknown_id = models.CharField(primary_key=True, max_length=255)
    process = models.ForeignKey(to=Process, on_delete=models.CASCADE, null=False)
    img_face = models.ImageField(null=True, upload_to='images/temps/', max_length=1023)
    confidence = models.FloatField(null=True, default=0)

    def __str__(self):
        return str(self.unknown_id)

    class Meta:
        verbose_name = 'Unknown'
        verbose_name_plural = 'Unknowns'
