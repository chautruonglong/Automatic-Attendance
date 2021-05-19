@REM python manage.py makemigrations user
@REM python manage.py migrate user
python manage.py makemigrations account
python manage.py migrate account
python manage.py makemigrations lecturer
python manage.py migrate lecturer
@REM python manage.py makemigrations room
@REM python manage.py migrate room

@REM python manage.py makemigrations student
@REM python manage.py migrate student
@REM python manage.py makemigrations emdedding
@REM python manage.py migrate emdedding
@REM python manage.py makemigrations subject
@REM python manage.py migrate subject   
@REM python manage.py makemigrations attendance
@REM python manage.py migrate attendance
@REM python manage.py makemigrations process
@REM python manage.py migrate process ListCreateAPIView