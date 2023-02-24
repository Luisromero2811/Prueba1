using System;
using Prueba1.Shared.DTOs;

namespace Prueba1.Client.Auth
{
	//Construccion de servicio para manejar la logica de inicio de sesion y de registro
	public interface ILoginService
	{
		//Con este method nos aseguramos de estar guardando el token en el LocalStorage
		Task Login(UserTokenDTO tokenDTO);
		Task Logout();
		Task ManejarRenovacionToken();
	}
}

