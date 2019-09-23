using System;
using AutoMapper;
using Xayah.Data;
using Xayah.Models;

namespace Xayah.AutoMapperConfig
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<TransactionViewModel, Transaction>()
                .ConstructUsing(c => new Transaction(c.BankId, c.AccountNumber, c.AccountType, c.Type, c.DatePosted, c.Amount, c.Memo));
        }
    }
}