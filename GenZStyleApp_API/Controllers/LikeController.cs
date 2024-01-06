using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenZStyleApp_API.Controllers
{
    public class LikeController : ODataController
    {
        private ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        #region Like
        [HttpGet("odata/Likes/{key}/Like")]
        [EnableQuery]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            GetPostLikeResponse like = await this._likeRepository.GetLikeByPostIdAsync(key);
            return Ok(like);
        }


        #endregion
    }
}
