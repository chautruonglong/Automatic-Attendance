from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework import status
from camera.api.camera import camera


@api_view(['GET'])
def open_camera_api(request, video):
    try:
        camera.open(video)

        return Response(
            data={
                'message': 'Camera is open'
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
