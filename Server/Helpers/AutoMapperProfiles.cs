using System;
using AutoMapper;
using Prueba1.Shared.DTOs;
using Prueba1.Shared.Controllers;


namespace Prueba1.Server.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		//Constructor
		public AutoMapperProfiles()
		{
			//Funcion createmap para mapear las entidades del actor e indicar el ignorar su foto
			CreateMap<Actores, Actores>()
				.ForMember(x => x.Foto, option => option.Ignore());

            CreateMap<BlazorPeliculas, BlazorPeliculas>()
                .ForMember(x => x.poster, option => option.Ignore());

			CreateMap<VotoPeliculaDTO, VotoPelicula>();
        }
	}
}

