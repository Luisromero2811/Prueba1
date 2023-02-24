using System;
using Microsoft.JSInterop;
namespace Prueba1.Client.Helpers
{
	public static class IJSRuntimeExtensionMethods
	{
		//Methods que permitirán interactuar con LocalStorage
		public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js, string llave, string contenido)
		{
			return js.InvokeAsync<object>("localStorage.setItem", llave, contenido);
		}
        public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.getItem", llave);
        }
        public static ValueTask<object> RemoverDeLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", llave);
        }
    }
}

