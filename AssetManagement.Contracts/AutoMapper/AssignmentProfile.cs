using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.AutoMapper
{
    public class AssignmentProfile: Profile
    {
        public AssignmentProfile()
        {
            CreateMap<AssetManagement.Domain.Models.Assignment, AssignmentResponse>()
                .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate.ToShortDateString()))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.ReturnedDate.ToShortDateString()));
        }
    }
}
