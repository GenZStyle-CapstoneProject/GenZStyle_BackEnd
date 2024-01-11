using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenZStyleApp_API.Controllers
{
    public class HashTagsController : ODataController
    {
        private IHashTagRepository _hashTagRepository;
        /*private IValidator<> */

        public HashTagsController(IHashTagRepository hashTagRepository)
        {
            _hashTagRepository = hashTagRepository;

        }

        #region SearchByHashTagName
        [HttpGet("odata/HashTags/{hashtag}/SearchByHashTagName")]

        public async Task<IActionResult> SearchByHashTagName([FromRoute] string hashtag)
        {
            try
            {
                List<GetHashTagReponse> hashtagDTOs = await _hashTagRepository.SearchByHashTagName(hashtag);

                // Nếu muốn thực hiện bất kỳ xử lý hoặc kiểm tra nào đó trước khi trả kết quả, bạn có thể thêm vào đây

                if (hashtagDTOs.Count > 0)
                {
                    // Thành công, trả về thông báo thành công và danh sách tài khoản
                    return Ok(new { Message = "Find HashTagName Successfully.", HashTags = hashtagDTOs });
                }
                else
                {
                    // Không tìm thấy tài khoản, trả về thông báo không có kết quả
                    return Ok(new { Message = "Not Found HashTagName in the system." });
                }
            }
            catch (Exception ex)
            {
                // Có lỗi, trả về thông báo lỗi
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }
        #endregion
        #region GetHashTags
        [EnableQuery]
        [HttpGet("odata/Hashtags/{key}")]
        public async Task<IActionResult> Get()
        {
                
            var result = await _hashTagRepository.GetHashTagsAsync();
            return Ok(result);
        }
        
        #endregion
    }
}

