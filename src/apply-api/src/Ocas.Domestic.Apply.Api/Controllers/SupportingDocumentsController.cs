using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SupportingDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportingDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/download")]
        public async Task<FileContentResult> Get(Guid id)
        {
            var binaryDocument = await _mediator.Send(new GetSupportingDocumentFile
            {
                User = User,
                Id = id
            });
            return File(binaryDocument.Data, binaryDocument.MimeType, binaryDocument.Name);
        }

        [HttpGet]
        public async Task<ActionResult<IList<SupportingDocument>>> GetSupportingDocuments(Guid applicantId)
        {
            return Ok(await _mediator.Send(new GetSupportingDocuments
            {
                ApplicantId = applicantId,
                User = User
            }));
        }
    }
}