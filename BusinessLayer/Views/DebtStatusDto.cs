using System;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class DebtStatusDto : IMapFrom<DebtStatus>
    {
        public long Id { get; set; }
        public string DebtStatusName { get; set; }
    }
}
