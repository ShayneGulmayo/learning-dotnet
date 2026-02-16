using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepository = commentRepo;
            _stockRepository = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentDto = comments.Select(c => c.ToCommentDto()).ToList();
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!await _stockRepository.StockExistsAsync(stockId))
            {
                return BadRequest($"Stock with id {stockId} does not exist.");
            }
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepository.CreateCommentAsync(commentModel);
            return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id}:int")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UpdateCommentRequestDto updateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.UpdateCommentAsync(id, updateDto.ToCommentFromUpdate());
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}:int")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.DeleteCommentAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment);
        }
    }
}