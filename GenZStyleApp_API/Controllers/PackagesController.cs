using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProjectParticipantManagement.BAL.Exceptions;

namespace GenZStyleApp_API.Controllers
{
    public class PackagesController : ODataController
    {
        public IPackageRepository _packageRepository;

        public PackagesController(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        [HttpPost("odata/Puchare/PurcharePackage")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> PurcharePackage(int PackageId)
        {
           
               try {
                    GetPackageResponse packageResponse = await this._packageRepository.PurcharePackage(PackageId, HttpContext);
                
                return Ok(new
                {
                    Status = "Purchare Package Success",
                    Data = Created(packageResponse)
                }); 
                }catch (Exception ex) 
                
                {
                    throw new Exception(ex.Message); 
                }
                
            }
            

        }

    }

