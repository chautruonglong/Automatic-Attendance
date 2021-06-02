from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework import status
from camera.api.camera import camera


@api_view(['GET'])
def capture_camera_api(request):
    try:
        jpg_text = camera.capture()

        return Response(
            data={
                'frame': jpg_text
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
