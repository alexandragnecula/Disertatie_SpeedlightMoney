using System;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class LoanStatusDto : IMapFrom<LoanStatus>
    {
        public long Id { get; set; }
        public string LoanStatusName { get; set; }
        public bool Deleted { get; set; }
    }
}
