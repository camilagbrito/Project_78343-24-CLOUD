using App.ViewModels;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Business.Interfaces;
using Business.Models;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-posts")]
    public class AdminPostsController : Controller
    {
        private const string url = "https://project78343images.blob.core.windows.net/postsimages/";
        private const string ContainerName = "postsimages";
        private readonly string _blobConnectionString;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPostsController(IPostRepository postRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userManager = userManager;
            _blobConnectionString = configuration.GetConnectionString("BlobConnectionString");
        }

        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetPostsUsersAndComments()));
        }

        [Route("delete-post/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = _mapper.Map<PostViewModel>(await _postRepository.GetbyId(id));

            if (id == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete-post/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var post = _mapper.Map<PostViewModel>(await _postRepository.GetbyId(id));

            if (id == null)
            {
                return NotFound();
            }

            if (post.Image != null)
            {
                DeleteFileStorage(post.Image);
            }

            await _postRepository.Delete(id);

            return RedirectToAction(nameof(List));
        }


        private async void DeleteFileStorage(string file)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);


            string[] path = file.Split("/");

            string blobfile = path[path.Length - 1];

            BlobClient image = blobContainerClient.GetBlobClient(blobfile);

            if (await image.ExistsAsync() && image.GetBlobLeaseClient() != null)
            {
                await image.DeleteAsync();
            }
        }
    }
}
