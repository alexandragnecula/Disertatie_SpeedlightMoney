using System;
using AutoMapper;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class CurrencyDto : IMapFrom<Currency>
    {
        public long Id { get; set; }
        public string CurrencyName { get; set; }
        public bool Deleted { get; set; }
    }
}
