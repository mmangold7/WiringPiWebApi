using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WiringPi;

namespace WiringPiApi.Controllers
{
    [Route("api/[controller]")]
    public class SignalsController : Controller
    {
        private bool SquareWaveOn { get; set; }
        private bool PwmOn { get; set; }
        private int SquareWaveDefaultPin { get; set; }
        private const int PWM_DEFAULT_PIN = 2;
        private const int SQUARE_WAVE_DEFAULT_PIN = 2;

        [HttpPost]
        public string SquareWave(double frequencyInHertz)
        {
            SquareWaveOn = true;
            SquareWaveAsync(frequencyInHertz);
            return $"Generating a square wave of frequency {frequencyInHertz} hertz";
        }
        
        private async Task SquareWaveAsync(double frequencyInHertz, int pin = SQUARE_WAVE_DEFAULT_PIN)
        {
            var periodInSeconds = 1 / frequencyInHertz;
            var periodInMicroseconds = (uint)(periodInSeconds * 1000000);
            var halfPeriodInMicroseconds = periodInMicroseconds / 2;

            Init.WiringPiSetupGpio();

            while (SquareWaveOn)
            {
                GPIO.digitalWrite(pin, 1);
                Timing.delayMicroseconds(halfPeriodInMicroseconds);
                GPIO.digitalWrite(pin, 0);
                Timing.delayMicroseconds(halfPeriodInMicroseconds);
            }
        }

        public string Pwm(double frequencyInHertz, double dutyCyclePercentage)
        {
            PwmOn = true;
            PwmAsync(frequencyInHertz, dutyCyclePercentage);
            return $"Generating a PWM signal of frequency {frequencyInHertz} hertz and {dutyCyclePercentage} percent duty cycle";
        }

        private async Task PwmAsync(double frequencyInHertz, double dutyCyclePercentage, int pin = PWM_DEFAULT_PIN)
        {
            var periodInSeconds = 1 / frequencyInHertz;
            var periodInMicroseconds = (uint)(periodInSeconds * 1000000);
            var onTime = (uint)(periodInMicroseconds * dutyCyclePercentage);
            var offTime = (periodInMicroseconds - onTime);

            Init.WiringPiSetupGpio();

            while (PwmOn)
            {
                GPIO.digitalWrite(pin, 1);
                Timing.delayMicroseconds(onTime);
                GPIO.digitalWrite(pin, 0);
                Timing.delayMicroseconds(offTime);
            }
        }

        [HttpPost]
        public string TurnOffSquareWave()
        {
            SquareWaveOn = false;
            return "Square wave generation turned off";
        }

        [HttpPost]
        public string TurnOffPwm()
        {
            PwmOn = false;
            return "PWM generation turned off";
        }

    }
}
