# Generated by Django 2.2.5 on 2021-05-30 09:40

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0018_auto_20210530_1625'),
    ]

    operations = [
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 4, 1, 0, 0)),
        ),
    ]