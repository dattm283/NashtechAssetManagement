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
using AssetManagement.Contracts.Assignment.Response;

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
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentDetailResponse>()
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => 
                    ((AssetManagement.Domain.Enums.Assignment.State)src.State).ToString()))
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => src.Asset.Specification))
                .ForMember(dest => dest.AssignToAppUser, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignByAppUser, opt => opt.MapFrom(src => src.AssignedByToAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, UpdateAssignmentResponse>();
        }
    }
}
