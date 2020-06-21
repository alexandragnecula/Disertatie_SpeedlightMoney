using System;
using AutoMapper;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class TransactionHistoryDto : IMapFrom<TransactionHistory>
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedOn { get; set; }

        //Sender
        public long SenderId { get; set; }
        public string SenderName { get; set; }

        //Beneficiar
        public long BeneficiarId { get; set; }
        public string BeneficiarName { get; set; }

        //Currency
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TransactionHistory, TransactionHistoryDto>()
                 .ForMember(d => d.SenderName,
                    opt => opt.MapFrom(s =>
                        s.Sender != null
                            ? s.Sender.GetFullName()
                            : string.Empty))
                  .ForMember(d => d.BeneficiarName,
                    opt => opt.MapFrom(s =>
                        s.Beneficiar != null
                            ? s.Beneficiar.GetFullName()
                            : string.Empty))
                  .ForMember(d => d.CurrencyName,
                        opt => opt.MapFrom(s =>
                            s.Currency != null
                                ? s.Currency.CurrencyName
                                : string.Empty));


        }
    }
}
