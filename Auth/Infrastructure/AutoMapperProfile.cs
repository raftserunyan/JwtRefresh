using System.Threading.Tasks;
using Auth.DTOs;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using AutoMapper;

namespace Auth.Infrastructure
{
	public sealed class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			RegisterMappings();
		}

		private void RegisterMappings()
		{
			CreateMap<RegisterViewModel, User>()
				.ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
				.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
				.ForAllOtherMembers(opt => opt.Ignore());

			CreateMap<Role, RoleViewModel>();

			CreateMap<User, UserViewModel>()
				.ForMember(dest => dest.Role, opts => opts.MapFrom(src => new RoleViewModel { Name = src.Role.Name }));

			CreateMap<User, UserDto>();

			CreateMap<UserDto, UserViewModel>()
				.ForMember(dest => dest.RefreshToken, opts => opts.MapFrom(src => src.RefreshToken.Token))
				.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.AccessToken))
				.ForMember(dest => dest.Role, opts => opts.MapFrom(src => new RoleViewModel { Name = src.Role.Name }));
		}
	}
}
