using App.ViewModels;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Route("challenges")]
        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<ChallengeViewModel>>(await _challengeRepository.GetAll()));
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
            var allChallenges = _mapper.Map<IEnumerable<ChallengeViewModel>>(await _challengeRepository.GetAll());

            if (!ModelState.IsValid)
            {
                return View(challengeViewModel);
            }

            foreach (var challenge in allChallenges)
            {
                if (challenge.Date == challengeViewModel.Date)
                {
                    TempData["Message"] = "Já existe um desafio cadastrado para esta data.";
                    return View(challengeViewModel);
                }
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

            foreach (var challenge in allChallenges)
            {
                if (challenge.Date == challengeViewModel.Date)
                {
                    return View(challengeViewModel);
                }
            }

            await _challengeRepository.Add(_mapper.Map<Challenge>(challengeViewModel));

            return RedirectToAction(nameof(List));
        }


        [Route("edit-challenge/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var challengeViewModel = _mapper.Map<ChallengeViewModel>(await _challengeRepository.GetbyId(id));

            if (challengeViewModel == null)
            {
                return NotFound();
            }

            return View(challengeViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-challenge/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, ChallengeViewModel challengeViewModel)
        {

            if (id != challengeViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(challengeViewModel);
            }

            if (challengeViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";


                if (!await UploadFileStorage(challengeViewModel.ImageUpload, imgPrefix))
                {
                    return View(challengeViewModel);
                }

                if (challengeViewModel.Image != null)
                {
                    await DeleteFileStorage(challengeViewModel.Image);
                }

                challengeViewModel.Image = url + imgPrefix + challengeViewModel.ImageUpload.FileName;
            }

            await _challengeRepository.Update(_mapper.Map<Challenge>(challengeViewModel));
            return RedirectToAction(nameof(List));
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

        private async Task<bool> DeleteFileStorage(string file)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            if (file == null) return false;

            string[] path = file.Split("/");

            string blobfile = path[path.Length - 1];

            BlobClient image = blobContainerClient.GetBlobClient(blobfile);

            if (await image.ExistsAsync() && image.GetBlobLeaseClient() != null)
            {
                await image.DeleteAsync();
            }
            return true;
        }
    }
}
