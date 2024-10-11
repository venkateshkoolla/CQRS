using System;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    /// <summary>
    /// Marker Interface to signal the pipeline behavior
    /// to check if the college user is valid
    /// </summary>
    public interface ICollegeUser : IIdentityUser
    {
        Guid CollegeId { get; }
    }
}
