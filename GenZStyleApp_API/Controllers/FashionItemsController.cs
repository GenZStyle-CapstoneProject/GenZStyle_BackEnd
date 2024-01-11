using GenZStyleAPP.BAL.DTOs.FashionItems;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class FashionItemsController : ControllerBase
    {
        private IFashionItemRepository _fashionItemRepository;
        
        public FashionItemsController(IFashionItemRepository repository)
        {
            this._fashionItemRepository = repository;
        }

        #region SearchByFashionName
        [HttpGet("odata/FashionItems/{fashion}/SearchByFashionName")]
        [EnableQuery]
        public async Task<IActionResult> SearchByFashionName([FromRoute] string fashion)
        {
            try
            {
                List<GetFashionItemResponse> fashionDTOs = await _fashionItemRepository.SearchByFashionName(fashion);

                // Nếu muốn thực hiện bất kỳ xử lý hoặc kiểm tra nào đó trước khi trả kết quả, bạn có thể thêm vào đây

                if (fashionDTOs.Count > 0)
                {
                    // Thành công, trả về thông báo thành công và danh sách tài khoản
                    return Ok(new { Message = "Find FashionName Successfully.", HashTags = fashionDTOs });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Not Found FashionName in the system." });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
            #endregion
        }
    }
}
