"""
:author chautruonglong
"""

from django.conf.urls.static import static
from django.contrib import admin
from django.urls import path
from drf_yasg import openapi
from drf_yasg.views import get_schema_view
from rest_framework import permissions

from backend_api import settings
from mobile.account import login_account, signup_account
from mobile.account import update_password, reset_password
from mobile.attendance import create_process, state_process
from mobile.attendance import detail_history
from mobile.attendance import list_attendance, list_history
from mobile.lecturer import detail_lecturer, update_lecturer
from mobile.room import list_room, update_room, create_room
from mobile.student import list_student
from mobile.subject import list_subject, create_subject, list_time_slot
from mobile.attendance import history_latest, history_attendance
from mobile.attendance import update_attendance

swagger_view = get_schema_view(
    openapi.Info(title='Automatic Attendance', default_version=''),
    public=True,
    permission_classes=(permissions.AllowAny, )
)

urlpatterns = [
    # Admin site
    path('', admin.site.urls),

    # Swagger
    path('swagger/', swagger_view.with_ui()),

    # Account
    path('account/login/', login_account.login_api),
    path('account/password/update/', update_password.update_password_api),
    path('account/password/reset/<str:email>/', reset_password.reset_password_api),
    path('account/password/reset/<str:email_b64>/<str:token>/', reset_password.reset_password_form_api),
    path('account/signup/', signup_account.signup_api),
    path('account/signup/activate/<str:email_b64>/<str:token>/', signup_account.active_account_api),

    # Lecturer
    path('lecturer/detail/<str:lecturer_id>/', detail_lecturer.detail_lecturer_api),
    path('lecturer/update/', update_lecturer.update_lecturer_api),

    # Subject
    path('subject/list/<str:lecturer_id>/<str:day>/', list_subject.list_subject_api),
    path('subject/create/', create_subject.create_subject_api),
    path('subject/time_slot/list/<str:room_id>/<str:day>/', list_time_slot.list_time_slot_api),

    # Student
    path('student/list/<str:subject_id>/', list_student.list_student_api),

    # Room
    path('room/list/', list_room.list_room_api),
    path('room/update/', update_room.update_room_api),
    path('room/create/', create_room.create_room_api),

    # Process
    path('attendance/process/create/<str:subject_id>/', create_process.create_process_api),
    path('attendance/process/state/<str:process_id>/', state_process.state_process_api),

    # Attendance
    path('attendance/list/<str:process_id>/', list_attendance.list_attendance_api),
    path('attendance/update/<str:subject_id>/<str:process_id>/', update_attendance.update_attendance_api),
    path('attendance/history/list/<str:subject_id>/<str:date>/', list_history.list_history_api),
    path('attendance/history/latest/<str:subject_id>/<str:date>/', history_latest.history_latest_api),
    path('attendance/history/detail/<str:process_id>/', detail_history.detail_history_api),
    path('attendance/history/subject/<str:subject_id>/', history_attendance.history_attendance_api),
]

urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
