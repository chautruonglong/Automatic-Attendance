from RPi import GPIO


class Led:
    _PIN_LED = 11

    def __init__(self):
        GPIO.setwarnings(False)
        GPIO.setmode(GPIO.BOARD)
        GPIO.setup(self._PIN_LED, GPIO.OUT)

    def turn_on(self):
        GPIO.output(self._PIN_LED, GPIO.HIGH)

    def turn_off(self):
        GPIO.output(self._PIN_LED, GPIO.LOW)
