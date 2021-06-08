from rest_framework.decorators import api_view, permission_classes
from rest_framework_api_key.permissions import HasAPIKey
from rest_framework.response import Response
from rest_framework import status
from django.shortcuts import render
from pandas import DataFrame
from backend_api.utils import get_base_url, convert_date, convert_time
from backend_api.settings import MEDIA_URL
from datetime import datetime
from core.models import Subject, StudentSubject, Student, Process, Attendance, Lecturer
from pdfkit import from_string
import os


@api_view(['GET'])
@permission_classes((HasAPIKey, ))
def history_attendance_api(request, subject_id):
    try:
        df = DataFrame()
        yes_path = f'{get_base_url(request)}/resources/icons/icon-yes.svg'
        no_path = f'{get_base_url(request)}/resources/icons/icon-no.svg'

        stusubs = StudentSubject.objects.filter(subject_id=subject_id)
        student_ids = []
        names = []
        for stusub in stusubs:
            student = Student.objects.get(student_id=stusub.student_id)
            student_ids.append(student.student_id)
            names.append(student.name)

        df['MSSV'] = student_ids
        df['Họ Tên'] = names

        dates = Process.objects.filter(subject_id=subject_id, state=True).values_list('date', flat=True).distinct()
        dates = sorted(dates)
        for date in dates:
            processes = Process.objects.filter(subject_id=subject_id, state=True, date=str(date))
            process = processes.order_by('-time').first()

            stusub_ids = Attendance.objects.filter(process_id=process.process_id).values_list('stusub_id', flat=True).distinct()
            checks = []
            for stusub in stusubs:
                if stusub.stusub_id in stusub_ids:
                    checks.append(1)
                else:
                    checks.append(0)
            df[date] = checks

        df['Có Mặt'] = df.iloc[:, 2:].sum(axis=1)
        df['Có Mặt'] = [f'{x}/{len(dates)}' for x in df['Có Mặt']]
        df.iloc[:, 2:-1] = df.iloc[:, 2:-1].applymap(lambda x: yes_path if x == 1 else no_path)

        subject = Subject.objects.get(subject_id=subject_id)
        lecturer = Lecturer.objects.get(lecturer_id=subject.lecturer_id)
        context = {
            'name': subject.name,
            'subject_id': subject_id,
            'time_slot': subject.time_slot,
            'day': subject.day,
            'lecturer': lecturer.name,
            'data_df': df
        }
        buffer = render(request, 'statistics.html', context).content.decode('utf-8')

        now = datetime.now()
        date_now = convert_date(now.date())
        time_now = convert_time(now.time())

        url = f'{MEDIA_URL}statistics/pdf/{subject_id}'
        if not os.path.exists(url):
            os.makedirs(url)

        pdf_url = f'{url}/{subject_id}_{date_now}_{time_now}.pdf'
        options = {
            'encoding': 'UTF-8',
            'orientation': 'portrait',
            'page-size': 'A4'
        }

        if 5 < len(dates) <= 9:
            options['orientation'] = 'landscape'
        elif len(dates) > 9:
            options['orientation'] = 'landscape'
            options['page-size'] = 'A3'

        from_string(buffer, pdf_url, options=options)

        return Response(
            data=f'{get_base_url(request)}/{pdf_url}',
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
