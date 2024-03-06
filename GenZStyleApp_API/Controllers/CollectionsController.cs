using GenZStyleAPP.BAL.DTOs.Collections;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CollectionsController : ODataController
    {
        private ICollectionRepository _collectionRepository;

        public CollectionsController(ICollectionRepository collectionRepository) 
        { 
            this._collectionRepository = collectionRepository;
        }


        #region Save Collection By PostId
        [HttpPost("odata/Posts/{postId}/SaveCollectionByPostId")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> SaveCollectionByPostId([FromRoute] int postId)
        {

            try
            {
                HttpContext httpContext = HttpContext;
                GetCollectionResponse collection   = await this._collectionRepository.SavePostToCollection(postId, httpContext);
                if (collection != null)
                {
                    return Ok(new { Message = "Save Post Into Post Successfully.", collectionResponses = collection });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Save Post Into Post Fail" });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion
    }
}
