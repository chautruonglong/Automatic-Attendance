# Generated by Django 2.2.5 on 2021-05-27 17:48

import datetime

import django.db.models.deletion
from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Lecturer',
            fields=[
                ('lecturer_id', models.BigAutoField(primary_key=True, serialize=False)),
                ('name', models.CharField(max_length=1023, null=True)),
                ('phone', models.CharField(max_length=255, null=True)),
                ('faculty', models.CharField(blank=True, choices=[('CNTT', 'Công Nghệ Thông Tin'), ('KHDL', 'Khoa Học Dữ Liệu'), ('DTVT', 'Điện Tử Viễn Thông'), ('CNSH', 'Công Nghệ Sinh Học'), ('KT', 'Kiến Trúc'), ('XD', 'Xây Dựng'), ('CD', 'Cầu Đường'), (None, 'Ngành Khác')], max_length=1023, null=True)),
            ],
            options={
                'verbose_name': 'Lecturer',
                'verbose_name_plural': 'Lecturers',
            },
        ),
        migrations.CreateModel(
            name='Room',
            fields=[
                ('room_id', models.PositiveIntegerField(primary_key=True, serialize=False)),
                ('zone', models.CharField(choices=[('a', 'A'), ('b', 'B'), ('c', 'C'), ('d', 'D'), ('e', 'E'), ('f', 'F'), ('g', 'G'), ('h', 'H'), (None, 'Khu Khác')], max_length=255, null=True)),
                ('ip_cam', models.CharField(max_length=255, null=True)),
            ],
            options={
                'verbose_name': 'Room',
                'verbose_name_plural': 'Rooms',
            },
        ),
        migrations.CreateModel(
            name='Student',
            fields=[
                ('student_id', models.CharField(max_length=255, primary_key=True, serialize=False)),
                ('name', models.CharField(max_length=1023, null=True)),
                ('phone', models.CharField(max_length=255, null=True)),
                ('birth', models.DateField(default=datetime.datetime(2000, 1, 15, 0, 0))),
                ('img_3x4', models.ImageField(null=True, upload_to='images/temps/')),
            ],
            options={
                'verbose_name': 'Student',
                'verbose_name_plural': 'Students',
            },
        ),
        migrations.CreateModel(
            name='Subject',
            fields=[
                ('subject_id', models.CharField(max_length=255, primary_key=True, serialize=False)),
                ('name', models.CharField(max_length=1023, null=True)),
                ('time_slot', models.CharField(choices=[('1', '7:00-7:50'), ('2', '8:00-8:50'), ('3', '9:00-9:50'), ('4', '10:00-10:50'), ('5', '11:00-11:50'), ('6', '12:30-13:20'), ('6', '13:30-14:20'), ('6', '14:30-15:20'), ('6', '15:30-16:20'), ('6', '16:30-17:20'), (None, 'Giờ Khác')], max_length=255, null=True)),
                ('day', models.CharField(choices=[('2', 'Monday'), ('3', 'Monday'), ('4', 'Monday'), ('5', 'Monday'), ('6', 'Monday'), ('7', 'Monday'), ('8', 'Monday'), (None, 'Thứ Khác')], max_length=255, null=True)),
                ('lecturer', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Lecturer')),
                ('room', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Room')),
            ],
            options={
                'verbose_name': 'Subject',
                'verbose_name_plural': 'Subjects',
            },
        ),
        migrations.CreateModel(
            name='StudentSubject',
            fields=[
                ('stusub_id', models.BigAutoField(primary_key=True, serialize=False)),
                ('student', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Student')),
                ('subject', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Subject')),
            ],
            options={
                'verbose_name': 'StudentSubject',
                'verbose_name_plural': 'StudentSubjects',
            },
        ),
        migrations.CreateModel(
            name='Process',
            fields=[
                ('process_id', models.BigAutoField(primary_key=True, serialize=False)),
                ('state', models.BooleanField(default=False)),
                ('date', models.DateField(auto_now=True)),
                ('time', models.TimeField(auto_now=True)),
                ('subject', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Subject')),
            ],
            options={
                'verbose_name': 'Process',
                'verbose_name_plural': 'Processes',
            },
        ),
        migrations.CreateModel(
            name='Attendance',
            fields=[
                ('attendance_id', models.BigAutoField(primary_key=True, serialize=False)),
                ('img_face', models.ImageField(null=True, upload_to='images/temps/')),
                ('process', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Process')),
                ('stusub', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.StudentSubject')),
            ],
            options={
                'verbose_name': 'Attendance',
                'verbose_name_plural': 'Attendances',
            },
        ),
        migrations.CreateModel(
            name='Account',
            fields=[
                ('email', models.EmailField(max_length=1023, primary_key=True, serialize=False)),
                ('hash_pwd', models.CharField(max_length=1023)),
                ('is_active', models.BooleanField(default=False)),
                ('lecturer', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='core.Lecturer')),
            ],
            options={
                'verbose_name': 'Account',
                'verbose_name_plural': 'Accounts',
            },
        ),
    ]
