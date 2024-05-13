using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Security.Policy;

namespace App.Controllers
{
    [Authorize]
    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Route("my-post-comments/{id:guid}")]
        public async Task<IActionResult> PostComments(Guid id)
        {
            var commentsViewModel = new List<CommentViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                var post = await _postRepository.GetPostandUserById(id);

                if (user.Id == post.UserId)
                {
                    var comments = _mapper.Map<IEnumerable<CommentViewModel>>(await _commentRepository.GetCommentsAndUserByPostId(id));
                    
                    foreach (var comment in comments)
                    {
                        var commentViewModel = new CommentViewModel
                        {
                            Id = comment.Id,
                            User = _mapper.Map<ApplicationUserViewModel>(comment.User),
                            CreatedDate = comment.CreatedDate,
                            Message = comment.Message,
                        };

                        commentsViewModel.Add(commentViewModel);
                    }
                    return View(commentsViewModel);
                }
                else
                {
                    return RedirectToAction("List", "Posts");
                }
            }
            return RedirectToAction("List", "Posts");
        }



        [Route("edit-my-comment/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var commentViewModel = _mapper.Map<CommentViewModel>(await _commentRepository.GetbyId(id));

            if (commentViewModel == null)
            {
                return NotFound();
            }

            return View(commentViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-my-comment/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CommentViewModel commentViewModel)
        {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.Id == commentViewModel.UserId)
            {

                if (id != commentViewModel.Id)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return View(commentViewModel);
                }

                await _commentRepository.Update(_mapper.Map<Comment>(commentViewModel));
            }

            return RedirectToAction("List", "Posts");
        }

        [Route("delete-comment/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = _mapper.Map<CommentViewModel>(await _commentRepository.GetbyId(id));

            if (id == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete-comment/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var comment = _mapper.Map<CommentViewModel>(await _commentRepository.GetbyId(id));

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.Id == comment.UserId)
            {

                if (id == null)
                {
                    return NotFound();
                }

                await _commentRepository.Delete(id);
            }

            return RedirectToAction("List", "Posts");
        }

    }
}
