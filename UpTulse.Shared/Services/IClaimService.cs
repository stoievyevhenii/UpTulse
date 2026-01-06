using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Shared.Services
{
    public interface IClaimService
    {
        string GetClaim(string key);

        string GetUserId();

        string GetUserRole();
    }
}