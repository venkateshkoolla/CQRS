//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ocas.Domestic.Crm.Entities
{
	
	/// <summary>
	/// Service endpoint that can be contacted.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("serviceendpoint")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class ServiceEndpoint : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string ComponentState = "componentstate";
			public const string ConnectionMode = "connectionmode";
			public const string Contract = "contract";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string Description = "description";
			public const string IntroducedVersion = "introducedversion";
			public const string IsCustomizable = "iscustomizable";
			public const string IsManaged = "ismanaged";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string Name = "name";
			public const string OrganizationId = "organizationid";
			public const string OverwriteTime = "overwritetime";
			public const string Path = "path";
			public const string ServiceEndpointId = "serviceendpointid";
			public const string Id = "serviceendpointid";
			public const string ServiceEndpointIdUnique = "serviceendpointidunique";
			public const string SolutionId = "solutionid";
			public const string SolutionNamespace = "solutionnamespace";
			public const string UserClaim = "userclaim";
			public const string createdby_serviceendpoint = "createdby_serviceendpoint";
			public const string lk_serviceendpointbase_createdonbehalfby = "lk_serviceendpointbase_createdonbehalfby";
			public const string lk_serviceendpointbase_modifiedonbehalfby = "lk_serviceendpointbase_modifiedonbehalfby";
			public const string modifiedby_serviceendpoint = "modifiedby_serviceendpoint";
			public const string organization_serviceendpoint = "organization_serviceendpoint";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public ServiceEndpoint() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "serviceendpoint";
		
		public const string EntitySchemaName = "ServiceEndpoint";
		
		public const string PrimaryIdAttribute = "serviceendpointid";
		
		public const string PrimaryNameAttribute = "name";
		
		public const int EntityTypeCode = 4618;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
		public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
			}
		}
		
		/// <summary>
		/// Connection mode to contact the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionmode")]
		public Microsoft.Xrm.Sdk.OptionSetValue ConnectionMode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("connectionmode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ConnectionMode");
				this.SetAttributeValue("connectionmode", value);
				this.OnPropertyChanged("ConnectionMode");
			}
		}
		
		/// <summary>
		/// Type of the endpoint contract.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("contract")]
		public Microsoft.Xrm.Sdk.OptionSetValue Contract
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("contract");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Contract");
				this.SetAttributeValue("contract", value);
				this.OnPropertyChanged("Contract");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Date and time when the service endpoint was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who created the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CreatedOnBehalfBy");
				this.SetAttributeValue("createdonbehalfby", value);
				this.OnPropertyChanged("CreatedOnBehalfBy");
			}
		}
		
		/// <summary>
		/// Description of the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("description", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Version in which the form is introduced.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
		public string IntroducedVersion
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("introducedversion");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IntroducedVersion");
				this.SetAttributeValue("introducedversion", value);
				this.OnPropertyChanged("IntroducedVersion");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component can be customized.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
		public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsCustomizable");
				this.SetAttributeValue("iscustomizable", value);
				this.OnPropertyChanged("IsCustomizable");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component is managed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
		public System.Nullable<bool> IsManaged
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Date and time when the service endpoint was last modified.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who modified the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ModifiedOnBehalfBy");
				this.SetAttributeValue("modifiedonbehalfby", value);
				this.OnPropertyChanged("ModifiedOnBehalfBy");
			}
		}
		
		/// <summary>
		/// Name of Service end point.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
		public string Name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Name");
				this.SetAttributeValue("name", value);
				this.OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// Unique identifier of the organization with which the service endpoint is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		public Microsoft.Xrm.Sdk.EntityReference OrganizationId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
		public System.Nullable<System.DateTime> OverwriteTime
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
			}
		}
		
		/// <summary>
		/// Path to the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("path")]
		public string Path
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("path");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Path");
				this.SetAttributeValue("path", value);
				this.OnPropertyChanged("Path");
			}
		}
		
		/// <summary>
		/// Unique identifier of the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointid")]
		public System.Nullable<System.Guid> ServiceEndpointId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("serviceendpointid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ServiceEndpointId");
				this.SetAttributeValue("serviceendpointid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ServiceEndpointId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointid")]
		public override System.Guid Id
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return base.Id;
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.ServiceEndpointId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the service endpoint.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceendpointidunique")]
		public System.Nullable<System.Guid> ServiceEndpointIdUnique
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("serviceendpointidunique");
			}
		}
		
		/// <summary>
		/// Unique identifier of the associated solution.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
		public System.Nullable<System.Guid> SolutionId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
			}
		}
		
		/// <summary>
		/// Namespace of the App Fabric solution.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionnamespace")]
		public string SolutionNamespace
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("solutionnamespace");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SolutionNamespace");
				this.SetAttributeValue("solutionnamespace", value);
				this.OnPropertyChanged("SolutionNamespace");
			}
		}
		
		/// <summary>
		/// Additional user claim value type.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userclaim")]
		public Microsoft.Xrm.Sdk.OptionSetValue UserClaim
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("userclaim");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UserClaim");
				this.SetAttributeValue("userclaim", value);
				this.OnPropertyChanged("UserClaim");
			}
		}
		
		/// <summary>
		/// 1:N serviceendpoint_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("serviceendpoint_sdkmessageprocessingstep")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SdkMessageProcessingStep> serviceendpoint_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SdkMessageProcessingStep>("serviceendpoint_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("serviceendpoint_sdkmessageprocessingstep");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SdkMessageProcessingStep>("serviceendpoint_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("serviceendpoint_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// 1:N userentityinstancedata_serviceendpoint
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_serviceendpoint")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_serviceendpoint
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_serviceendpoint", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_serviceendpoint");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_serviceendpoint", null, value);
				this.OnPropertyChanged("userentityinstancedata_serviceendpoint");
			}
		}
		
		/// <summary>
		/// N:1 createdby_serviceendpoint
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("createdby_serviceendpoint")]
		public Ocas.Domestic.Crm.Entities.SystemUser createdby_serviceendpoint
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("createdby_serviceendpoint", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_serviceendpointbase_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_serviceendpointbase_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_serviceendpointbase_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_serviceendpointbase_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_serviceendpointbase_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_serviceendpointbase_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_serviceendpointbase_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_serviceendpointbase_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_serviceendpointbase_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_serviceendpointbase_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_serviceendpointbase_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_serviceendpointbase_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_serviceendpointbase_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_serviceendpointbase_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 modifiedby_serviceendpoint
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("modifiedby_serviceendpoint")]
		public Ocas.Domestic.Crm.Entities.SystemUser modifiedby_serviceendpoint
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("modifiedby_serviceendpoint", null);
			}
		}
		
		/// <summary>
		/// N:1 organization_serviceendpoint
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_serviceendpoint")]
		public Ocas.Domestic.Crm.Entities.Organization organization_serviceendpoint
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_serviceendpoint", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
		public virtual ComponentState? ComponentStateEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ComponentState?)(EntityOptionSetEnum.GetEnum(this, "componentstate")));
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionmode")]
		public virtual ServiceEndpoint_ConnectionMode? ConnectionModeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ServiceEndpoint_ConnectionMode?)(EntityOptionSetEnum.GetEnum(this, "connectionmode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				ConnectionMode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("contract")]
		public virtual ServiceEndpoint_Contract? ContractEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ServiceEndpoint_Contract?)(EntityOptionSetEnum.GetEnum(this, "contract")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				Contract = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userclaim")]
		public virtual ServiceEndpoint_UserClaim? UserClaimEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ServiceEndpoint_UserClaim?)(EntityOptionSetEnum.GetEnum(this, "userclaim")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				UserClaim = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}