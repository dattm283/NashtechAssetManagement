using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Domain.Enums.AppUser;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Domain.Enums.Assignment;

namespace AssetManagement.Contracts.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserResponse>();
            CreateMap<AppUser, UpdateUserResponse>();
           
            CreateMap<AppUser, ViewListUser_UserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StaffCode))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedDate.Date))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName))
                .ForMember(dest => dest.ValidAssignments, opt => opt.MapFrom(src => src.AssignedToAssignments.Count()));
            CreateMap<AppUser, ViewDetailUser_UserResponse>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (UserGender)src.Gender))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => (AppUserLocation)src.Location))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Dob))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedDate.Date))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<AppUser, DeleteUserResponse>();
        }
    }
}
