"""
:author chautruonglong
"""

from django.shortcuts import render
from django.utils import timezone
from django.db import transaction
from rest_framework.response import Response
from rest_framework.decorators import api_view, permission_classes
from rest_framework import status
from rest_framework_api_key.models import APIKey
from rest_framework_api_key.permissions import HasAPIKey
from core.models import Subject, StudentSubject, Student
from backend_api.utils import convert_date_django, generate_date


@api_view(['POST'])
@permission_classes((HasAPIKey, ))
def create_subject_api(request):
    try:
        body = request.data
        subject_id = body['subject_id']
        lecturer_id = body['lecturer_id']
        room_id = body['room_id']
        subject_name = body['name']
        time_slot = body['time_slot']
        day = body['day']

        if Subject.objects.filter(subject_id=subject_id).exists():
            raise Exception('Subject is existing')

        with transaction.atomic():
            Subject.objects.create(
                subject_id=subject_id,
                lecturer_id=lecturer_id,
                room_id=room_id,
                name=subject_name,
                time_slot=time_slot,
                day=day
            )

            for student in body['students']:
                student_id = student['student_id']
                student_name = student['name']
                phone = student['phone']
                birth = student['birth']
                class_name = student['class_name']
                img_3x4 = f'images/originals/it/k18/{student_id}.jpg'

                if not Student.objects.filter(student_id=student_id).exists():
                    Student.objects.create(
                        student_id=student_id,
                        name=student_name,
                        phone=phone,
                        birth=convert_date_django(birth),
                        class_name=class_name,
                        img_3x4=img_3x4
                    )

                StudentSubject.objects.create(
                    subject_id=subject_id,
                    student_id=student_id
                )

        return Response(
            data={
                'message': 'Create successfully'
            },
            status=status.HTTP_202_ACCEPTED
        )

    except Exception as error:
        print(str(error))
        return Response(
            data={
                'error_message': str(error)
            },
            status=status.HTTP_406_NOT_ACCEPTABLE
        )
