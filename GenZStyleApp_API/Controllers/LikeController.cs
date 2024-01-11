using BMOS.BAL.Authorization;
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
        [HttpGet("odata/PostLikes/GetPostId/{key}")]
        [EnableQuery]
        [PermissionAuthorize("User")]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            GetPostLikeResponse like = await this._likeRepository.GetLikeByPostIdAsync(key, HttpContext);
            return Ok(like);
        }


        #endregion
    }
}
