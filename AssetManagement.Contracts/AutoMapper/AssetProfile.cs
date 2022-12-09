using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AutoMapper;
using AssetManagement.Domain.Enums.Assignment;
namespace AssetManagement.Contracts.AutoMapper
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Domain.Models.Asset, ViewListAssetsResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.IsEditable,
                    opt => opt.MapFrom(src => !(src.Assignments.Where(x=>!x.IsDeleted)
                    .Any(x => (x.State == State.Accepted || x.State == State.WaitingForAcceptance)) ||
                    src.State == AssetManagement.Domain.Enums.Asset.State.Assigned)));
            CreateMap<Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<CreateAssetRequest, Domain.Models.Asset>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Specification, opt => opt.MapFrom(src => src.Specification.Trim()));
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, CreateAssetRequest>();

            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, CreateAssetRequest>();
        }
    }
}
