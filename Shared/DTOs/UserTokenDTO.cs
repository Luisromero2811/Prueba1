using System;
namespace Prueba1.Shared.DTOs
{
	public class UserTokenDTO
	{
		public string Token { get; set; } = null!;
		public DateTime Expiration { get; set; }
	}
}

