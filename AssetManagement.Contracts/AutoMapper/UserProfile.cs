using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Domain.Models;
using AssetManagement.Contracts.Asset.Request;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Domain.Enums.AppUser;

namespace AssetManagement.Contracts.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, ViewListAssets_AssetResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<AssetManagement.Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, CreateAssetRequest>();
            CreateMap<CreateAssetRequest, AssetManagement.Domain.Models.Asset>();

            CreateMap<AppUser, ViewListUser_UserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedDate.Date))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));
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
