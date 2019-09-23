using AutoMapper;
using Xayah.Data;
using Xayah.Models;

namespace Xayah.AutoMapperConfig
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Transaction, TransactionViewModel>();
        }
    }
}
