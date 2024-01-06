using FluentValidation;
using GenZStyleAPP.BAL.Repository.Interfaces;
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
