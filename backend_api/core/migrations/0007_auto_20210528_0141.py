# Generated by Django 2.2.5 on 2021-05-27 18:41

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0006_auto_20210528_0138'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='zone',
            field=models.CharField(max_length=255, null=True),
        ),
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 5, 11, 0, 0)),
        ),
    ]
