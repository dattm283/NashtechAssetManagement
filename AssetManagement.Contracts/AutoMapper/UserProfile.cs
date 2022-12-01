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

namespace AssetManagement.Contracts.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserResponse>();
            CreateMap<AppUser, UpdateUserResponse>();
            
            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, ViewListAssets_AssetResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<AssetManagement.Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, CreateAssetRequest>();
            CreateMap<CreateAssetRequest, AssetManagement.Domain.Models.Asset>();
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentDetailResponse>()
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => 
                    ((AssetManagement.Domain.Enums.Assignment.State)src.State).ToString()))
                .ForMember(dest => dest.AssignToAppUserStaffCode, opt => opt.MapFrom(src => 
                    src.AssignedToAppUser.StaffCode))
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => src.Asset.Specification))
                .ForMember(dest => dest.AssignToAppUser, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignByAppUser, opt => opt.MapFrom(src => src.AssignedByToAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, UpdateAssignmentResponse>();
            CreateMap<AppUser, ViewListUser_UserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StaffCode))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedDate.Date))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<AppUser, ViewDetailUser_UserResponse>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (UserGender)src.Gender))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => (AppUserLocation)src.Location))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Dob))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedDate.Date))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + ' ' + src.LastName));
            CreateMap<AppUser, DeleteUserResponse>();
            CreateMap<AssetManagement.Domain.Models.Assignment, ViewListAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByToAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.ToShortDateString()));
        }
    }
}
