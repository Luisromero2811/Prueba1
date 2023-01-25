using System;
namespace Prueba1.Client.Helpers
{
//Se crea un tipo de datos para mandar clave-valor
	public class SelectorMultipleModel
	{
		public SelectorMultipleModel(string llave, string valor)
		{
			Llave = llave;
			Valor = valor;
		}

		public string Llave { get; set; }
		public string Valor { get; set; }
	
	}
}

