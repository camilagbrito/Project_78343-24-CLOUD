using App.ViewModels;
using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-challenge")]
    public class AdminChallengesController : Controller
    {
        private const string url = "https://project78343images.blob.core.windows.net/challengeimages/";
        private const string ContainerName = "challengeimages";
        private readonly string _blobConnectionString;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IMapper _mapper;

        public AdminChallengesController(IChallengeRepository challengeRepository, IMapper mapper, IConfiguration configuration)
        {
            _challengeRepository = challengeRepository;
            _mapper = mapper;
            _blobConnectionString = configuration.GetConnectionString("BlobConnectionString");
        }

        [Route("new-challenge")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-challenge")]
        public async Task<IActionResult> Create(ChallengeViewModel challengeViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(challengeViewModel);
            }

            var imgPrefix = Guid.NewGuid() + "_";

            if (!await UploadFileStorage(challengeViewModel.ImageUpload, imgPrefix))
            {
                return View(challengeViewModel);
            }

            if (challengeViewModel.ImageUpload != null)
            {
                challengeViewModel.Image = url + imgPrefix + challengeViewModel.ImageUpload.FileName;
            }

            await _challengeRepository.Add(_mapper.Map<Challenge>(challengeViewModel));

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UploadFileStorage(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0 || file == null) return false;

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
    }
}
