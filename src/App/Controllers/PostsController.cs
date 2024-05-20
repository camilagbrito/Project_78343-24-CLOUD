using App.ViewModels;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace App.Controllers
{
    [Route("posts/")]
    [Authorize]
    public class PostsController : Controller
    {
        private const string url = "https://project78343images.blob.core.windows.net/postsimages/";
        private const string ContainerName = "postsimages";
        private readonly string _blobConnectionString;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _blobConnectionString = configuration.GetConnectionString("BlobConnectionString");
        }

        [AllowAnonymous]
        [Route("posts-list")]
        public async Task<IActionResult> List()
        {

            return View(_mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetPostsUsersAndComments()));

        }

        [Route("my-posts")]
        public async Task<IActionResult> UserPosts()
        {
            var postsViewModel = new List<PostViewModel>();
           
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var posts = await _postRepository.GetPostsAndCommentsByUserId(userId);

                foreach (var post in posts)
                {
                    var postViewModel = new PostViewModel
                    {
                        Id = post.Id,
                        User = _mapper.Map<ApplicationUserViewModel>(post.User),
                        CreatedDate = post.CreatedDate,
                        Title = post.Title,
                        Content = post.Content,
                        Image = post.Image
                    };

                    postsViewModel.Add(postViewModel);
                }

                if (postsViewModel.IsNullOrEmpty())
                {
                    TempData["Empty"] = "Ainda não possui posts.";
                }

                return View(postsViewModel);
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }
        }

        [Route("new-post")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-post")]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (!ModelState.IsValid)
                {
                    return View(postViewModel);
                }

                var imgPrefix = Guid.NewGuid() + "_";

                await UploadFileStorage(postViewModel.ImageUpload, imgPrefix);

                if (postViewModel.ImageUpload != null)
                {
                    postViewModel.Image = url + imgPrefix + postViewModel.ImageUpload.FileName;
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                postViewModel.CreatedDate = DateTime.Now;
                postViewModel.UserId = user.Id;

                await _postRepository.Add(_mapper.Map<Post>(postViewModel));
            }

            return RedirectToAction(nameof(List));
        }

        [Route("edit-my-post/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var postViewModel = _mapper.Map<PostViewModel>(await _postRepository.GetbyId(id));

            if (postViewModel == null)
            {
                return NotFound();
            }

            return View(postViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-my-post/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, PostViewModel postViewModel)
        {

            var postUpdate = _mapper.Map<PostViewModel>(await _postRepository.GetPostandUserById(id));

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.Id == postUpdate.UserId)
            {

                if (id != postViewModel.Id)
                {
                    return NotFound();
                }

                postViewModel.CreatedDate = postUpdate.CreatedDate;
                postViewModel.Image = postUpdate.Image;
                postViewModel.UserId = postUpdate.UserId;

                if (!ModelState.IsValid)
                {
                    return View(postViewModel);
                }

                if (postViewModel.ImageUpload != null)
                {
                    var imgPrefix = Guid.NewGuid() + "_";


                    if (!await UploadFileStorage(postViewModel.ImageUpload, imgPrefix))
                    {
                        return View(postViewModel);
                    }

                    if (postViewModel.Image != null)
                    {
                        DeleteFileStorage(postViewModel.Image);
                    }

                    postViewModel.Image = url + imgPrefix + postViewModel.ImageUpload.FileName;
                }

                await _postRepository.Update(_mapper.Map<Post>(postViewModel));
            }

            return RedirectToAction(nameof(UserPosts));
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

            var post = _mapper.Map<PostViewModel>(await _postRepository.GetPostandUserById(id));

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.Id == post.UserId)
            {

                if (id == null)
                {
                    return NotFound();
                }

                DeleteFileStorage(post.Image);

                await _postRepository.Delete(id);
            }

            return RedirectToAction(nameof(UserPosts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-comment")]
        public async Task<IActionResult> AddComment(CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                commentViewModel.CreatedDate = DateTime.Now;
                commentViewModel.UserId = user.Id;

                await _commentRepository.Add(_mapper.Map<Comment>(commentViewModel));
            }

            return RedirectToAction("List", "Posts");
        }
        private async Task<bool> UploadFileStorage(IFormFile file, string imgPrefix)
        {
            if (file == null) return false;

            var name = imgPrefix + file.FileName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(name);

            BlobHttpHeaders headers = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(file.OpenReadStream(), headers);

            return true;

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
