using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions.Models;
using Core.Helpers;
using Core.Model;
using Core.Models.File;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Handler;
using Kantin.Models.Request;
using Kantin.Models.Response;
using Kantin.Service.Attributes;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuItemController : Controller
    {
        private readonly KantinEntities _entities;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MenuItemController(KantinEntities entities, IMapper mapper, IConfiguration configuration)
        {
            _entities = entities;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<MenuItem>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get([FromQuery]Query query)
        {
            using var service = new MenuItemsProvider(_entities);
            var result = await service.GetAll(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using var service = new MenuItemsProvider(_entities);
            var result = await service.Get(id);
            var response = _mapper.Map<EditableMenuItemResponse>(result);
            return Ok(response);
        }

        [HttpPost]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]EditableMenuItemRequest editableMenuItem)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var menuItem = _mapper.Map<MenuItem>(editableMenuItem);
                var result = await service.Create(menuItem);
                var response = _mapper.Map<EditableMenuItemResponse>(result);
                return Created($"api/menuItem/{result.Id}", response);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]EditableMenuItemRequest editableMenuItem)
        {
            var menuItem = _mapper.Map<MenuItem>(editableMenuItem);
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new MenuItemsProvider(_entities, accountIdentity);
            var result = await service.Update(id, menuItem);
            var response = _mapper.Map<EditableMenuItemResponse>(result);
            return Ok(response);
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
            using var service = new MenuItemsProvider(_entities, accountIdentity);
            var result = await service.Delete(id);
            if (result)
                return NoContent();
                
            return NotFound();
        }

        [HttpPost("{id}/Upload")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Upload(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);

            if (!accountIdentity.AccountId.HasValue || !accountIdentity.OrganisationId.HasValue)
                return Unauthorized();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var files = Request.Form.Files;
            var result = new List<UploadResult>();

            using var service = new MenuItemAttachmentsProvider(_entities, accountIdentity);
            var organisationId = accountIdentity.OrganisationId.Value;

            foreach (var file in files)
            {
                var data = file.OpenReadStream();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                var attachment = await service.Create(new MenuItemAttachment
                {
                    FileName = fileName,
                    OrganisationId = organisationId,
                    MenuItemId = id
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

        [HttpGet("{id}/Download/{attachmentId}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Download(Guid id, Guid attachmentId)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);

            if (!accountIdentity.AccountId.HasValue || !accountIdentity.OrganisationId.HasValue)
                return Unauthorized();
            using var service = new MenuItemAttachmentsProvider(_entities, accountIdentity);
            var organisationId = accountIdentity.OrganisationId.Value;

            var attachment = await service.Get(attachmentId);
            if (attachment.MenuItemId != id)
                return NotFound();

            var fileStorageHelper = new FileStorageHelper(_configuration);
            var downloadResult = await fileStorageHelper.Download(organisationId, attachmentId, attachment.FileName);
            var mimeType = FileHelpers.GetMimeType(downloadResult.FileName);
            return new FileStreamResult(downloadResult.Data, mimeType)
            {
                FileDownloadName = downloadResult.FileName
            };
        }
    }
}
