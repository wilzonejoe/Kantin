using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Core.Exceptions.Models;
using Core.Helpers;
using Core.Models.File;
using Kantin.Data;
using Kantin.Service.Attributes;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly KantinEntities _entities;

        public FileController(KantinEntities entities, IConfiguration configuration)
        {
            _entities = entities;
            _configuration = configuration;
        }

        [HttpPost]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post()
        {
            var accountIdentity =
                AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            if (!accountIdentity.AccountId.HasValue)
                return Unauthorized();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var files = Request.Form.Files;
            var result = new List<UploadResult>();

            foreach (var file in files)
            {
                var data = file.OpenReadStream();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                var uploadResult = await fileStorageHelper.Upload(new UploadFile
                {
                    AttachmentId = Guid.NewGuid(),
                    OrganisationId = Guid.NewGuid(),
                    FileName = fileName,
                    Data = data
                });

                result.Add(uploadResult);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(File))]
        [ProducesResponseType((int) HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            var accountIdentity =
                AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            if (!accountIdentity.AccountId.HasValue)
                return Unauthorized();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var downloadResult = await fileStorageHelper.Download(accountIdentity.AccountId.Value, id, "test.txt");
            return File(downloadResult.Data, MediaTypeNames.Application.Octet, downloadResult.FileName);
        }
    }
}