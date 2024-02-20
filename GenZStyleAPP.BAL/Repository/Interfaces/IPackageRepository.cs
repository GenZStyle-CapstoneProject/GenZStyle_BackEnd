using GenZStyleAPP.BAL.DTOs.Package;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IPackageRepository
    {
        public Task<GetPackageResponse> PurcharePackage(int PackageId, HttpContext httpContext);
    }
}
