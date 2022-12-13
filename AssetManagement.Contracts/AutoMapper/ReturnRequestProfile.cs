using AssetManagement.Contracts.ReturnRequest.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.AutoMapper
{
    public class ReturnRequestProfile : Profile
    {
        public ReturnRequestProfile()
        {
            CreateMap<Domain.Models.ReturnRequest, ViewListReturnRequestResponse>()
                .ForMember(dest => dest.AssetCode, opt => opt.MapFrom(src => src.Assignment.Asset.AssetCode))
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Assignment.Asset.Name))
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.AcceptedByUser.UserName))
                .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => src.AcceptedByUser.UserName));
            CreateMap<Domain.Models.ReturnRequest, CancelReturnRequestResponse>()
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.AssignedByUser.UserName))
                .ForMember(dest => dest.AcceptedBy, opt => opt.MapFrom(src => src.AcceptedByUser.UserName))
                .ForMember(dest => dest.AssetId, opt => opt.MapFrom(src => src.Assignment.Asset.Id));
        }
    }
}
