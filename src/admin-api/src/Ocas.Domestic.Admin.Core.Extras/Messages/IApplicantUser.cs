﻿using System;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    /// <summary>
    /// Marker Interface to signal the pipeline behavior
    /// to check if the applicant user is Valid
    /// </summary>
    public interface IApplicantUser : IIdentityUser
    {
        Guid ApplicantId { get; }
    }
}
