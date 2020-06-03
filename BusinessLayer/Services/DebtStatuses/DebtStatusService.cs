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

namespace BusinessLayer.Services.DebtStatuses
{
    public class DebtStatusService : IDebtStatusService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public DebtStatusService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddDebtStatus(DebtStatusDto debtStatusToAdd)
        {
            var entity = new DebtStatus
            {
                DebtStatusName = debtStatusToAdd.DebtStatusName,
            };

            await _context.DebtStatuses.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Debt status was created successfully");
        }

        public async Task<Result> UpdateDebtStatus(DebtStatusDto debtStatusToUpdate)
        {
            var entity = await _context.DebtStatuses
               .FirstOrDefaultAsync(x => x.Id == debtStatusToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt status found" });
            }

            entity.DebtStatusName = debtStatusToUpdate.DebtStatusName;

            await _context.SaveChangesAsync();

            return Result.Success("Debt status update was successful");
        }

        public async Task<Result> DeleteDebtStatus(DebtStatusDto debtStatusToDelete)
        {
            var entity = await _context.DebtStatuses
                .FirstOrDefaultAsync(x => x.Id == debtStatusToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt status found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Debt Status was deleted");
        }

        public async Task<Result> RestoreDebtStatus(DebtStatusDto debtStatusToRestore)
        {
            var entity = await _context.DebtStatuses
                .FirstOrDefaultAsync(x => x.Id == debtStatusToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid debt status found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Debt status was restored");
        }

        public async Task<DebtStatusDto> GetDebtStatusById(long id)
        {
            var entity = await _context.DebtStatuses.FindAsync(id);

            return entity == null ? null : _mapper.Map<DebtStatusDto>(entity);
        }

        public async Task<IList<DebtStatusDto>> GetAllDebtStatuses()
        {
            List<DebtStatusDto> debtStatuses = await _context.DebtStatuses
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<DebtStatusDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return debtStatuses;
        }

        public async Task<SelectItemVm> GetAllAsSelect(DebtStatusDto debtStatusDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.DebtStatuses
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.DebtStatusName, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
