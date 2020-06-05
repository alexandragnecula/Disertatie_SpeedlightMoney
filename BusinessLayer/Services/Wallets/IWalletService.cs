using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Wallets
{
    public interface IWalletService
    {
        Task<Result> AddWallet(WalletDto walletToAdd);
        Task<Result> UpdateWallet(WalletDto walletToUpdate);
        Task<Result> DeleteWallet(WalletDto walletToDelete);
        Task<Result> RestoreWallet(WalletDto walletToRestore);
        Task<WalletDto> GetWalletById(long id);
        Task<IList<WalletDto>> GetAllWallets();
        Task<IList<WalletDto>> GetWalletsForCurrentUser();
        Task<SelectItemVm> GetAllAsSelect(WalletDto walletDto);
    }
}
