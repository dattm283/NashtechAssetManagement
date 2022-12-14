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
            CreateMap<Domain.Models.Assignment, ViewListAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.Value.ToShortDateString()));
            CreateMap<Domain.Models.Assignment, AssignmentDetailResponse>()
               .ForMember(dest => dest.StateName, opt => opt.MapFrom(src =>
                   (src.State).ToString()))
               .ForMember(dest => dest.AssignToAppUserStaffCode, opt => opt.MapFrom(src =>
                   src.AssignedToAppUser.StaffCode))
               .ForMember(dest => dest.AssignToAppUserFullName, opt => opt.MapFrom(src =>
                   src.AssignedToAppUser.FirstName + " " + src.AssignedToAppUser.LastName))
               .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
               .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
               .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => src.Asset.Specification))
               .ForMember(dest => dest.AssignToAppUser, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
               .ForMember(dest => dest.AssignByAppUser, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<Domain.Models.Assignment, UpdateAssignmentResponse>();
            CreateMap<Domain.Models.Assignment, ViewListAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));
            CreateMap<Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.Value.ToShortDateString()));

            CreateMap<CreateAssignmentRequest, Domain.Models.Assignment>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note.Trim()));
            CreateMap<Domain.Models.Assignment, CreateAssignmentResponse>();

            CreateMap<AssetManagement.Domain.Models.Assignment, ViewListReturnRequestResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.AssignedToAppUser.UserName))
                .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => src.AssignedByAppUser.UserName));

            CreateMap<AssetManagement.Domain.Models.Assignment, MyAssignmentResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Asset.Category.Name));
            
            CreateMap<Domain.Models.Assignment, AcceptAssignmentResponse>()
                .ForMember(dest => dest.AssetState, opt => opt.MapFrom(src => src.Asset.State));
            CreateMap<Domain.Models.Assignment, DeclineAssignmentResponse>()
                .ForMember(dest => dest.AssetState, opt => opt.MapFrom(src => src.Asset.State));
        }
    }
}
