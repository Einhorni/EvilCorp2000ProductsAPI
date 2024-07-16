using AutoMapper;

namespace EvilCorp2000Products.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        { 
            CreateMap<Entities.Product, Models.ProductDTO>();
            CreateMap<Models.ProductForCreationDto, Entities.Product>();
            CreateMap<Models.ProductForUpdateDto, Entities.Product>();
            CreateMap<Entities.Product, Models.ProductForUpdateDto>();
        }
    }
}
