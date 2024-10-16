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
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public enum msdyn_wallsavedqueryState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Contains information regarding which views are available for users to display on their personal walls. Only an administrator can specify the views that users can choose from to display on their personal walls.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("msdyn_wallsavedquery")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class msdyn_wallsavedquery : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string msdyn_entitydisplayname = "msdyn_entitydisplayname";
			public const string msdyn_entityname = "msdyn_entityname";
			public const string msdyn_IsVirtual = "msdyn_isvirtual";
			public const string msdyn_IsVisible = "msdyn_isvisible";
			public const string msdyn_isvisiblebit = "msdyn_isvisiblebit";
			public const string msdyn_otc = "msdyn_otc";
			public const string msdyn_postconfigurationid = "msdyn_postconfigurationid";
			public const string msdyn_SavedQueryId = "msdyn_savedqueryid";
			public const string msdyn_savedqueryname = "msdyn_savedqueryname";
			public const string msdyn_wallsavedqueryId = "msdyn_wallsavedqueryid";
			public const string Id = "msdyn_wallsavedqueryid";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_msdyn_wallsavedquery_createdby = "lk_msdyn_wallsavedquery_createdby";
			public const string lk_msdyn_wallsavedquery_createdonbehalfby = "lk_msdyn_wallsavedquery_createdonbehalfby";
			public const string lk_msdyn_wallsavedquery_modifiedby = "lk_msdyn_wallsavedquery_modifiedby";
			public const string lk_msdyn_wallsavedquery_modifiedonbehalfby = "lk_msdyn_wallsavedquery_modifiedonbehalfby";
			public const string msdyn_postconfig_wallsavedquery = "msdyn_postconfig_wallsavedquery";
			public const string organization_msdyn_wallsavedquery = "organization_msdyn_wallsavedquery";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public msdyn_wallsavedquery() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "msdyn_wallsavedquery";
		
		public const string EntitySchemaName = "msdyn_wallsavedquery";
		
		public const string PrimaryIdAttribute = "msdyn_wallsavedqueryid";
		
		public const string PrimaryNameAttribute = "msdyn_entityname";
		
		public const int EntityTypeCode = 10170;
		
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
		/// Unique identifier of the user who created the record.
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
		/// Date and time when the record was created.
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
		/// Unique identifier of the delegate user who created the record.
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
		/// Sequence number of the import that created this record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
		public System.Nullable<int> ImportSequenceNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ImportSequenceNumber");
				this.SetAttributeValue("importsequencenumber", value);
				this.OnPropertyChanged("ImportSequenceNumber");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who modified the record.
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
		/// Date and time when the record was modified.
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
		/// Unique identifier of the delegate user who modified the record.
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
		/// Type of entity to which the corresponding views belong
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_entitydisplayname")]
		public string msdyn_entitydisplayname
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("msdyn_entitydisplayname");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_entitydisplayname");
				this.SetAttributeValue("msdyn_entitydisplayname", value);
				this.OnPropertyChanged("msdyn_entitydisplayname");
			}
		}
		
		/// <summary>
		/// Name of the entity to which the corresponding views belong.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_entityname")]
		public string msdyn_entityname
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("msdyn_entityname");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_entityname");
				this.SetAttributeValue("msdyn_entityname", value);
				this.OnPropertyChanged("msdyn_entityname");
			}
		}
		
		/// <summary>
		/// Information that indicates whether the entity is virtual, that is whether, it is generated by a plug-in, and no corresponding entity instance exists.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_isvirtual")]
		public System.Nullable<bool> msdyn_IsVirtual
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("msdyn_isvirtual");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_IsVirtual");
				this.SetAttributeValue("msdyn_isvirtual", value);
				this.OnPropertyChanged("msdyn_IsVirtual");
			}
		}
		
		/// <summary>
		/// Indicates that corresponding system view should be displayed on personal wall
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_isvisible")]
		public System.Nullable<bool> msdyn_IsVisible
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("msdyn_isvisible");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_IsVisible");
				this.SetAttributeValue("msdyn_isvisible", value);
				this.OnPropertyChanged("msdyn_IsVisible");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_isvisiblebit")]
		public System.Nullable<int> msdyn_isvisiblebit
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("msdyn_isvisiblebit");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_isvisiblebit");
				this.SetAttributeValue("msdyn_isvisiblebit", value);
				this.OnPropertyChanged("msdyn_isvisiblebit");
			}
		}
		
		/// <summary>
		/// Type of entity to which the corresponding views belong.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_otc")]
		public System.Nullable<int> msdyn_otc
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("msdyn_otc");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_otc");
				this.SetAttributeValue("msdyn_otc", value);
				this.OnPropertyChanged("msdyn_otc");
			}
		}
		
		/// <summary>
		/// Unique identifier for Post Configuration associated with Wall View.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_postconfigurationid")]
		public Microsoft.Xrm.Sdk.EntityReference msdyn_postconfigurationid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("msdyn_postconfigurationid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_postconfigurationid");
				this.SetAttributeValue("msdyn_postconfigurationid", value);
				this.OnPropertyChanged("msdyn_postconfigurationid");
			}
		}
		
		/// <summary>
		/// Unique identifier of the view that should be displayed on the personal wall for all users.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_savedqueryid")]
		public string msdyn_SavedQueryId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("msdyn_savedqueryid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_SavedQueryId");
				this.SetAttributeValue("msdyn_savedqueryid", value);
				this.OnPropertyChanged("msdyn_SavedQueryId");
			}
		}
		
		/// <summary>
		/// Name of the corresponding view.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_savedqueryname")]
		public string msdyn_savedqueryname
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("msdyn_savedqueryname");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_savedqueryname");
				this.SetAttributeValue("msdyn_savedqueryname", value);
				this.OnPropertyChanged("msdyn_savedqueryname");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_wallsavedqueryid")]
		public System.Nullable<System.Guid> msdyn_wallsavedqueryId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("msdyn_wallsavedqueryid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedqueryId");
				this.SetAttributeValue("msdyn_wallsavedqueryid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("msdyn_wallsavedqueryId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_wallsavedqueryid")]
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
				this.msdyn_wallsavedqueryId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier for the organization
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
		/// Date and time that the record was migrated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
		public System.Nullable<System.DateTime> OverriddenCreatedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OverriddenCreatedOn");
				this.SetAttributeValue("overriddencreatedon", value);
				this.OnPropertyChanged("OverriddenCreatedOn");
			}
		}
		
		/// <summary>
		/// Status of the Wall View
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the Wall View
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public Microsoft.Xrm.Sdk.OptionSetValue StatusCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StatusCode");
				this.SetAttributeValue("statuscode", value);
				this.OnPropertyChanged("StatusCode");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleversionnumber")]
		public System.Nullable<int> TimeZoneRuleVersionNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("timezoneruleversionnumber");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TimeZoneRuleVersionNumber");
				this.SetAttributeValue("timezoneruleversionnumber", value);
				this.OnPropertyChanged("TimeZoneRuleVersionNumber");
			}
		}
		
		/// <summary>
		/// Time zone code that was in use when the record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("utcconversiontimezonecode")]
		public System.Nullable<int> UTCConversionTimeZoneCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("utcconversiontimezonecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UTCConversionTimeZoneCode");
				this.SetAttributeValue("utcconversiontimezonecode", value);
				this.OnPropertyChanged("UTCConversionTimeZoneCode");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> msdyn_wallsavedquery_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("msdyn_wallsavedquery_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("msdyn_wallsavedquery_AsyncOperations", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> msdyn_wallsavedquery_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("msdyn_wallsavedquery_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("msdyn_wallsavedquery_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> msdyn_wallsavedquery_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("msdyn_wallsavedquery_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("msdyn_wallsavedquery_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> msdyn_wallsavedquery_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("msdyn_wallsavedquery_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("msdyn_wallsavedquery_ProcessSession", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> msdyn_wallsavedquery_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("msdyn_wallsavedquery_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("msdyn_wallsavedquery_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// 1:N msdyn_wallsavedquery_wallsavedqueryusersettings
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_wallsavedquery_wallsavedqueryusersettings")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryusersettings> msdyn_wallsavedquery_wallsavedqueryusersettings
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryusersettings>("msdyn_wallsavedquery_wallsavedqueryusersettings", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_wallsavedquery_wallsavedqueryusersettings");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.msdyn_wallsavedqueryusersettings>("msdyn_wallsavedquery_wallsavedqueryusersettings", null, value);
				this.OnPropertyChanged("msdyn_wallsavedquery_wallsavedqueryusersettings");
			}
		}
		
		/// <summary>
		/// N:1 lk_msdyn_wallsavedquery_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_msdyn_wallsavedquery_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_msdyn_wallsavedquery_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_msdyn_wallsavedquery_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_msdyn_wallsavedquery_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_msdyn_wallsavedquery_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_msdyn_wallsavedquery_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_msdyn_wallsavedquery_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_msdyn_wallsavedquery_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_msdyn_wallsavedquery_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_msdyn_wallsavedquery_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_msdyn_wallsavedquery_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_msdyn_wallsavedquery_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_msdyn_wallsavedquery_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_msdyn_wallsavedquery_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_msdyn_wallsavedquery_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_msdyn_wallsavedquery_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 msdyn_postconfig_wallsavedquery
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("msdyn_postconfigurationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("msdyn_postconfig_wallsavedquery")]
		public Ocas.Domestic.Crm.Entities.msdyn_PostConfig msdyn_postconfig_wallsavedquery
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.msdyn_PostConfig>("msdyn_postconfig_wallsavedquery", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("msdyn_postconfig_wallsavedquery");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.msdyn_PostConfig>("msdyn_postconfig_wallsavedquery", null, value);
				this.OnPropertyChanged("msdyn_postconfig_wallsavedquery");
			}
		}
		
		/// <summary>
		/// N:1 organization_msdyn_wallsavedquery
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_msdyn_wallsavedquery")]
		public Ocas.Domestic.Crm.Entities.Organization organization_msdyn_wallsavedquery
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_msdyn_wallsavedquery", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual msdyn_wallsavedquery_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((msdyn_wallsavedquery_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}