from django.contrib import admin

from core import models


class StudentAdmin(admin.ModelAdmin):
    list_display = ('student_id', 'name', 'phone', 'birth', 'class_name', 'img_3x4')


class LecturerAdmin(admin.ModelAdmin):
    list_display = ('lecturer_id', 'name', 'phone', 'faculty')


class AccountAdmin(admin.ModelAdmin):
    list_display = ('email', 'hash_pwd', 'is_active')


class RoomAdmin(admin.ModelAdmin):
    list_display = ('room_id', 'zone', 'ip_cam')
    readonly_fields = ('zone', )


class SubjectAdmin(admin.ModelAdmin):
    list_display = ('subject_id', 'lecturer_id', 'room_id', 'name', 'time_slot', 'day')


class StudentSubjectAdmin(admin.ModelAdmin):
    list_display = ('stusub_id', 'subject_id', 'student_id')


class ProcessAdmin(admin.ModelAdmin):
    list_display = ('process_id', 'subject_id', 'state', 'date', 'time')


class AttendanceAdmin(admin.ModelAdmin):
    list_display = ('attendance_id', 'stusub_id', 'process_id', 'img_face', 'confidence')


class UnknownAdmin(admin.ModelAdmin):
    list_display = ('unknown_id', 'process_id', 'img_face', 'confidence')


admin.site.register(models.Student, StudentAdmin)
admin.site.register(models.Lecturer, LecturerAdmin)
admin.site.register(models.Account, AccountAdmin)
admin.site.register(models.Room, RoomAdmin)
admin.site.register(models.Subject, SubjectAdmin)
admin.site.register(models.StudentSubject, StudentSubjectAdmin)
admin.site.register(models.Process, ProcessAdmin)
admin.site.register(models.Attendance, AttendanceAdmin)
admin.site.register(models.Unknown, UnknownAdmin)
