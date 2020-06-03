using System;
namespace BusinessLayer.Views
{
    public class TermDto
    {
        public long Id { get; set; }
        public string TermName { get; set; }
        public int PeriodInDays { get; set; }
        public bool Deleted { get; set; }
    }
}
