# Generated by Django 3.0.5 on 2021-05-08 05:36

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('student', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='student',
            name='id_number',
            field=models.IntegerField(),
        ),
    ]
