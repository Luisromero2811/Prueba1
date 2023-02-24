using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Prueba1.Client.Auth
{
	public class RenovadorToken : IDisposable
	{
        private readonly ILoginService loginService;

        public RenovadorToken(ILoginService loginService)
		{
            this.loginService = loginService;
        }

        Timer? timer;

        public void Iniciar()
        {
            timer = new Timer();
            timer.Interval = 1000 * 60 * 9; //4minutos
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            loginService.ManejarRenovacionToken();
        }

        public void Dispose()
        {
            //Se limpia el Timer
            timer?.Dispose();
        }
    }
}

