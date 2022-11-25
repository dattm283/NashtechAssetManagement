using AssetManagement.Contracts.Category.Request;
using AssetManagement.Contracts.Category.Response;
using AutoMapper;

namespace AssetManagement.Contracts.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Domain.Models.Category, GetCategoryResponse>();
            CreateMap<CreateCategoryRequest, Domain.Models.Category>();
        }
    }
}
