using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AutoMapper;

namespace AssetManagement.Contracts.AutoMapper
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Domain.Models.Asset, ViewListAssets_AssetResponse>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<CreateAssetRequest, Domain.Models.Asset>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, CreateAssetRequest>();
        }
    }
}
