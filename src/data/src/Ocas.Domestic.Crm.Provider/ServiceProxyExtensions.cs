using System;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Ocas.Domestic.Crm.Provider
{
    public static class ServiceProxyExtensions
    {
        public static Task<OrganizationResponse> ExecuteAsync(this OrganizationServiceProxy serviceProxy, OrganizationRequest request)
        {
            return Task.Run(() => serviceProxy.Execute(request));
        }

        public static Task<Entity> RetrieveAsync(this OrganizationServiceProxy serviceProxy, string entityName, Guid id, ColumnSet columnSet)
        {
            return Task.Run(() => serviceProxy.Retrieve(entityName, id, columnSet));
        }

        public static Task<EntityCollection> RetrieveMultipleAsync(this OrganizationServiceProxy serviceProxy, QueryBase query)
        {
            return Task.Run(() => serviceProxy.RetrieveMultiple(query));
        }

        public static Task DeactivateEntityAsync(this OrganizationServiceProxy serviceProxy, string entityName, Guid entityId)
        {
            return Task.Run(() =>
            {
                var request = new SetStateRequest
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = entityId,
                        LogicalName = entityName
                    },
                    State = new OptionSetValue((int)Constants.State.Inactive),
                    Status = new OptionSetValue((int)Constants.Status.Inactive)
                };
                serviceProxy.Execute(request);
            });
        }

        public static Task ActivateEntityAsync(this OrganizationServiceProxy serviceProxy, string entityName, Guid entityId)
        {
            return Task.Run(() =>
            {
                var request = new SetStateRequest
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = entityId,
                        LogicalName = entityName
                    },
                    State = new OptionSetValue((int)Constants.State.Active),
                    Status = new OptionSetValue((int)Constants.Status.Active)
                };
                serviceProxy.Execute(request);
            });
        }

        public static Task AssociateEntitiesAsync(this OrganizationServiceProxy serviceProxy, string entity1Name, Guid entity1Id, string entity2Name, Guid entity2Id, string relationshipName)
        {
            return Task.Run(() =>
            {
                var request = new AssociateEntitiesRequest
                {
                    Moniker1 = new EntityReference(entity1Name, entity1Id),
                    Moniker2 = new EntityReference(entity2Name, entity2Id),
                    RelationshipName = relationshipName
                };
                return serviceProxy.Execute(request);
            });
        }

        public static Task UnAssociateEntitiesAsync(this OrganizationServiceProxy serviceProxy, string entity1Name, Guid entity1Id, string entity2Name, Guid entity2Id, string relationshipName)
        {
            return Task.Run(() =>
            {
                var request = new DisassociateEntitiesRequest
                {
                    Moniker1 = new EntityReference(entity1Name, entity1Id),
                    Moniker2 = new EntityReference(entity2Name, entity2Id),
                    RelationshipName = relationshipName
                };
                return serviceProxy.Execute(request);
            });
        }

        public static Task<EntityCollection> FetchEntitiesAsync(this OrganizationServiceProxy serviceProxy, string fetchXml)
        {
            return Task.Run(() => serviceProxy.RetrieveMultiple(new FetchExpression(fetchXml)));
        }

        public static Task ActivateEmailEntityAsync(this OrganizationServiceProxy serviceProxy, Guid emailId)
        {
            return Task.Run(() =>
            {
                var sendEmailRequest = new SendEmailRequest
                {
                    EmailId = emailId,
                    TrackingToken = string.Empty,
                    IssueSend = true
                };

                return serviceProxy.Execute(sendEmailRequest);
            });
        }

        public static Task<EntityCollection> RetrieveMultipleEntitiesAsync(this OrganizationServiceProxy serviceProxy, QueryBase query)
        {
            return Task.Run(() => serviceProxy.RetrieveMultiple(query));
        }

        public static Task<Entity> RetrieveEntityAsync(this OrganizationServiceProxy serviceProxy, string entityName, Guid entityId)
        {
            return Task.Run(() => serviceProxy.Retrieve(entityName, entityId, new ColumnSet(true)));
        }

        public static Task<Entity> RetrieveEntityAsync(this OrganizationServiceProxy serviceProxy, string entityName, Guid entityId, ColumnSet columns)
        {
            return Task.Run(() => serviceProxy.Retrieve(entityName, entityId, columns));
        }
    }
}