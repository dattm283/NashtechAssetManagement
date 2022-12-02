using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AutoMapper;

namespace AssetManagement.Contracts.AutoMapper
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Domain.Models.Asset, ViewListAssetsResponse>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<CreateAssetRequest, Domain.Models.Asset>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<Domain.Models.Asset, CreateAssetRequest>();

            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, ViewListAssets_AssetResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, UpdateAssetRequest>();
            CreateMap<AssetManagement.Domain.Models.Asset, DeleteAssetReponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, GetAssetByIdResponse>();
            CreateMap<AssetManagement.Domain.Models.Asset, CreateAssetRequest>();
            CreateMap<CreateAssetRequest, AssetManagement.Domain.Models.Asset>();
        }
    }
}
