using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using ProjectParticipantManagement.BAL.Exceptions;

namespace GenZStyleApp_API.Controllers
{
    public class CommentsController : ODataController
    {
        private ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("odata/Meals/Active/Meal/{PostId}")]
        public async Task<IActionResult> Get(int PostId)
        {
            var result = await _commentRepository.GetCommentByPostId(PostId);
            return Ok(result);
        }

        [EnableQuery]
        [HttpPost("odata/Comment/Post/{key}")]

        public async Task<IActionResult> Post([FromRoute] int key, [FromForm]  GetCommentRequest commentRequest)
        {
            
            GetCommentResponse getCommentResponse = await this._commentRepository.UpdateCommentByPostId(commentRequest, key);
            return Created(getCommentResponse);
        }


    }
}
