using System;
namespace BusinessLayer.Views
{
    public class LoanStatusDto
    {
        public long Id { get; set; }
        public string LoanStatusName { get; set; }
        public bool Deleted { get; set; }
    }
}
