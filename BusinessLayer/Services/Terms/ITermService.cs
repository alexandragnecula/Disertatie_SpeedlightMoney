using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;


namespace BusinessLayer.Services.Terms
{
    public interface ITermService
    {
        Task<Result> AddTerm(TermDto termToAdd);
        Task<Result> UpdateTerm(TermDto termToUpdate);
        Task<Result> DeleteTerm(TermDto termToDelete);
        Task<Result> RestoreTerm(TermDto termToRestore);
        Task<TermDto> GetTermById(long id);
        Task<IList<TermDto>> GetAllTerms();
        Task<SelectItemVm> GetAllAsSelect(TermDto termDto);
    }
}
