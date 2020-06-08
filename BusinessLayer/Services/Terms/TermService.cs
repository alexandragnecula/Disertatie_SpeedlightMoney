using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Terms
{
    public class TermService : ITermService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public TermService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddTerm(TermDto termToAdd)
        {
            var entity = new Term
            {
                TermName = termToAdd.TermName,
                PeriodInDays = termToAdd.PeriodInDays,
               
            };

            await _context.Terms.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Term was created successfully");
        }

        public async Task<Result> UpdateTerm(TermDto termToUpdate)
        {
            var entity = await _context.Terms
               .FirstOrDefaultAsync(x => x.Id == termToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid term found" });
            }

            entity.TermName = termToUpdate.TermName;
            entity.PeriodInDays = termToUpdate.PeriodInDays;

            await _context.SaveChangesAsync();

            return Result.Success("Term update was successful");
        }

        public async Task<Result> DeleteTerm(TermDto termToDelete)
        {
            var entity = await _context.Terms
                 .FirstOrDefaultAsync(x => x.Id == termToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid term found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Term was deleted");
        }

        public async Task<Result> RestoreTerm(TermDto termToRestore)
        {
            var entity = await _context.Terms
                .FirstOrDefaultAsync(x => x.Id == termToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid term found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Term was restored");
        }

        public async Task<TermDto> GetTermById(long id)
        {
            var entity = await _context.Terms.FindAsync(id);

            return entity == null ? null : _mapper.Map<TermDto>(entity);
        }

        public async Task<IList<TermDto>> GetAllTerms()
        {
            List<TermDto> terms = await _context.Terms
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<TermDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return terms;
        }

        public async Task<SelectItemVm> GetAllAsSelect(TermDto termDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Terms
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.TermName, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
