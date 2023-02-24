using System;
using System.ComponentModel.DataAnnotations;
namespace Prueba1.Shared.DTOs
{
	public class UserInfo
	{
		[EmailAddress]
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}

