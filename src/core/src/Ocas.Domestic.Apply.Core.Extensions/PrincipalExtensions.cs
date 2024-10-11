using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetDisplayName(this IPrincipal user)
        {
            var displayName = "anonymous";

            if (!string.IsNullOrWhiteSpace(user?.Identity?.Name))
            {
                displayName = user.Identity.Name;
            }

            return displayName;
        }

        public static string GetSubject(this IPrincipal user)
        {
            return user?.Identity.GetClaim("sub")?.Value;
        }

        public static string GetUserId(this IPrincipal user)
        {
            var subject = user?.Identity.GetClaim("sub")?.Value?.Split(':');
            return subject?.Length > 0 ? subject[0] : null;
        }

        public static string GetUpnOrEmail(this IPrincipal user)
        {
            return user?.Identity.GetClaim("upn")?.Value
                ?? user?.Identity.GetClaim("email")?.Value;
        }

        public static string GetUserIdOrDefault(this IPrincipal user)
        {
            return user.GetUserId();
        }

        public static string GetIssuer(this IPrincipal user)
        {
            return user.GetClaimValueOrDefault("iss");
        }

        public static string GetIssuerPlusUserId(this IPrincipal user)
        {
            return user.GetClaimValueOrDefault("iss", "unknown") + user.GetUserId();
        }

        public static string GetPartnerId(this IPrincipal user)
        {
            var partnerId = user?.Identity.GetClaim("partner_id")?.Value;

            return !string.IsNullOrEmpty(partnerId) ? partnerId : null;
        }

        public static string GetIdp(this IPrincipal user)
        {
            return user?.Identity.GetClaim("idp")?.Value;
        }

        public static string GetClaimValueOrDefault(this IPrincipal user, string type, string defaultValue = null)
        {
            var claim = user?.Identity.GetClaim(type);

            if (claim == null)
            {
                return defaultValue;
            }

            return claim.Value;
        }

        public static string GetClaimValue(this IPrincipal user, string type)
        {
            var claim = user?.Identity.GetClaim(type);

            if (claim == null)
            {
                throw new ArgumentException("No Claim with that type", type);
            }

            return claim.Value;
        }

        public static Claim GetClaim(this IIdentity identity, string type)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == type);
        }

        public static IEnumerable<Claim> GetClaims(this IIdentity identity, string type)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity?.Claims?.Where(c => c.Type == type);
        }
    }
}
