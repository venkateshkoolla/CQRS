using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Ocas.Domestic.Crm.Provider
{
    public static class ServiceContextExtensions
    {
        public static Task<Guid> CreateEntityAsync(this OrganizationServiceContext serviceContext, Entity crmEntity)
        {
            return Task.Run(async () =>
            {
                serviceContext.AddObject(crmEntity);

                try
                {
                    await serviceContext.SaveChangesAsync();
                }
                finally
                {
                    // Detach this entity if it's causing problems, otherwise the DomesticContext becomes useless
                    // https://community.dynamics.com/crm/f/117/t/119865
                    serviceContext.Detach(crmEntity);
                }

                return crmEntity.Id;
            });
        }

        public static Task UpdateEntityAsync(this OrganizationServiceContext serviceContext, Entity crmEntity)
        {
            if (crmEntity == null) throw new ArgumentNullException(nameof(crmEntity));

            return Task.Run(async () =>
            {
                if (!serviceContext.IsAttached(crmEntity))
                {
                    var existingEntity = serviceContext.GetAttachedEntities().FirstOrDefault(e => e.Id == crmEntity.Id);
                    if (existingEntity != null)
                    {
                        serviceContext.Detach(existingEntity);
                    }

                    serviceContext.Attach(crmEntity);
                }

                serviceContext.UpdateObject(crmEntity);

                try
                {
                    await serviceContext.SaveChangesAsync();
                }
                finally
                {
                    // Detach this entity if it's causing problems, otherwise the DomesticContext becomes useless
                    // https://community.dynamics.com/crm/f/117/t/119865
                    serviceContext.Detach(crmEntity);
                }
            });
        }

        public static Task DeleteEntityAsync(this OrganizationServiceContext serviceContext, string entityName, Guid entityId)
        {
            return Task.Run(async () =>
            {
                var crmEntity = new Entity(entityName) { Id = entityId };
                if (!serviceContext.IsAttached(crmEntity))
                {
                    var existingEntity = serviceContext.GetAttachedEntities().FirstOrDefault(e => e.Id == crmEntity.Id);
                    if (existingEntity != null)
                    {
                        serviceContext.Detach(existingEntity);
                    }

                    serviceContext.Attach(crmEntity);
                }

                serviceContext.DeleteObject(crmEntity);

                try
                {
                    await serviceContext.SaveChangesAsync();
                }
                finally
                {
                    // Detach this entity if it's causing problems, otherwise the DomesticContext becomes useless
                    // https://community.dynamics.com/crm/f/117/t/119865
                    serviceContext.Detach(crmEntity);
                }
            });
        }

        private static Task<SaveChangesResultCollection> SaveChangesAsync(this OrganizationServiceContext context)
        {
            return Task.Run(() => context.SaveChanges());
        }
    }
}