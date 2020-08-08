using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Core.Exceptions.Models;
using Core.Helpers;
using Core.Model;
using Core.Models.File;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Handler;
using Kantin.Service.Attributes;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class AddOnItemController : Controller
    {
        private readonly KantinEntities _entities;
        private readonly IConfiguration _configuration;

        public AddOnItemController(KantinEntities entities, IConfiguration configuration)
        { 
            _entities = entities;
            _configuration = configuration;
        }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AddOnItem>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get([FromQuery]Query query)
        {
            using var service = new AddOnItemsProvider(_entities);
            var result = await service.GetAll(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using var service = new AddOnItemsProvider(_entities);
            var result = await service.Get(id);
            return Ok(result);
        }

        [HttpPost]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new AddOnItemsProvider(_entities, accountIdentity);
            var result = await service.Create(addOnItem);
            return Created($"api/addOnItem/{result.Id}", result);
        }

        [HttpPut("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new AddOnItemsProvider(_entities, accountIdentity);
            var result = await service.Update(id, addOnItem);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new AddOnItemsProvider(_entities, accountIdentity);
            var result = await service.Delete(id);
            if (result)
                return NoContent();

            return NotFound();
        }

        [HttpPost("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> UploadAttachment(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);

            if (!accountIdentity.AccountId.HasValue || !accountIdentity.OrganisationId.HasValue)
                return Unauthorized();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var files = Request.Form.Files;
            var result = new List<UploadResult>();

            using var service = new AddOnItemAttachmentsProvider(_entities, accountIdentity);
            var organisationId = accountIdentity.OrganisationId.Value;

            foreach (var file in files)
            {
                var data = file.OpenReadStream();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                var attachment = await service.Create(new AddOnItemAttachment
                {
                    FileName = fileName,
                    OrganisationId = organisationId,
                    AddOnItemId = id
                });

                var uploadResult = await fileStorageHelper.Upload(new UploadFile
                {
                    AttachmentId = attachment.Id,
                    OrganisationId = organisationId,
                    FileName = fileName,
                    Data = data
                });

                result.Add(uploadResult);
            }

            return Ok(result);
        }

        [HttpGet("{id}/Upload")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(File))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Download(Guid id, Guid attachmentId)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);

            if (!accountIdentity.AccountId.HasValue || !accountIdentity.OrganisationId.HasValue)
                return Unauthorized();
            using var service = new AddOnItemAttachmentsProvider(_entities, accountIdentity);
            var organisationId = accountIdentity.OrganisationId.Value;

            var attachment = await service.Get(attachmentId);
            if (attachment.AddOnItemId != id)
                return NotFound();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var downloadResult = await fileStorageHelper.Download(organisationId, attachmentId, attachment.FileName);
            var mimeType = FileHelper.GetMimeType(downloadResult.FileName);
            return new FileStreamResult(downloadResult.Data, mimeType)
            {
                FileDownloadName = downloadResult.FileName
            };
        }
    }
}
