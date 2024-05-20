using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-comments")]
    public class AdminCommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminCommentsController(ICommentRepository commentRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _userManager = userManager;
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

            if (id == null)
            {
                return NotFound();
            }

            await _commentRepository.Delete(id);

            return RedirectToAction("List", "Posts");
        }
    }
}
