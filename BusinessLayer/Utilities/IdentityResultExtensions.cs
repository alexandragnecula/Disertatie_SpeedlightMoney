using System;
using System.Linq;
using DataLayer.Shared;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Utilities
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
