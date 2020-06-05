using System;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class TermDto : IMapFrom<Term>
    {
        public long Id { get; set; }
        public string TermName { get; set; }
        public int PeriodInDays { get; set; }
        public bool Deleted { get; set; }
    }
}
