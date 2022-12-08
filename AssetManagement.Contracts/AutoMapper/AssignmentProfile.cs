using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.Assignment.Request;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Contracts.ReturnRequest.Response;

namespace AssetManagement.Contracts.AutoMapper
{
    public class AssignmentProfile: Profile
    {
        public AssignmentProfile()
        {
            CreateMap<AssetManagement.Domain.Models.Assignment, ViewListAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.ToShortDateString()));
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentDetailResponse>()
               .ForMember(dest => dest.StateName, opt => opt.MapFrom(src =>
                   ((AssetManagement.Domain.Enums.Assignment.State)src.State).ToString()))
               .ForMember(dest => dest.AssignToAppUserStaffCode, opt => opt.MapFrom(src =>
                   src.AssignedToAppUser.StaffCode))
               .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
               .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
               .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => src.Asset.Specification))
               .ForMember(dest => dest.AssignToAppUser, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
               .ForMember(dest => dest.AssignByAppUser, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, UpdateAssignmentResponse>();
            CreateMap<AssetManagement.Domain.Models.Assignment, ViewListAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.ToShortDateString()));

            CreateMap<CreateAssignmentRequest, AssetManagement.Domain.Models.Assignment>();
            CreateMap<AssetManagement.Domain.Models.Assignment, CreateAssignmentResponse>();

            CreateMap<AssetManagement.Domain.Models.Assignment, ViewListReturnRequestResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
        }
    }
}
