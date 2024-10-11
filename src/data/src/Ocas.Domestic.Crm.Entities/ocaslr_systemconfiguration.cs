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
	public enum ocaslr_systemconfigurationState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Used to store configurable settings for the applicant portal.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("ocaslr_systemconfiguration")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class ocaslr_systemconfiguration : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
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
			public const string ocaslr_key = "ocaslr_key";
			public const string ocaslr_modifiedbyuser = "ocaslr_modifiedbyuser";
			public const string ocaslr_name = "ocaslr_name";
			public const string ocaslr_systemconfigurationId = "ocaslr_systemconfigurationid";
			public const string Id = "ocaslr_systemconfigurationid";
			public const string Ocaslr_textvalue = "ocaslr_textvalue";
			public const string ocaslr_value = "ocaslr_value";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_ocaslr_systemconfiguration_createdby = "lk_ocaslr_systemconfiguration_createdby";
			public const string lk_ocaslr_systemconfiguration_createdonbehalfby = "lk_ocaslr_systemconfiguration_createdonbehalfby";
			public const string lk_ocaslr_systemconfiguration_modifiedby = "lk_ocaslr_systemconfiguration_modifiedby";
			public const string lk_ocaslr_systemconfiguration_modifiedonbehalfby = "lk_ocaslr_systemconfiguration_modifiedonbehalfby";
			public const string organization_ocaslr_systemconfiguration = "organization_ocaslr_systemconfiguration";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public ocaslr_systemconfiguration() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "ocaslr_systemconfiguration";
		
		public const string EntitySchemaName = "ocaslr_systemconfiguration";
		
		public const string PrimaryIdAttribute = "ocaslr_systemconfigurationid";
		
		public const string PrimaryNameAttribute = "ocaslr_name";
		
		public const int EntityTypeCode = 10038;
		
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
		/// The key (name) of the setting.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_key")]
		public string ocaslr_key
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_key");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_key");
				this.SetAttributeValue("ocaslr_key", value);
				this.OnPropertyChanged("ocaslr_key");
			}
		}
		
		/// <summary>
		/// For auditing purposes,  CRM attribute that need to track which user insert, update or delete current entity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_modifiedbyuser")]
		public string ocaslr_modifiedbyuser
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_modifiedbyuser");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_modifiedbyuser");
				this.SetAttributeValue("ocaslr_modifiedbyuser", value);
				this.OnPropertyChanged("ocaslr_modifiedbyuser");
			}
		}
		
		/// <summary>
		/// The name of the custom entity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_name")]
		public string ocaslr_name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_name");
				this.SetAttributeValue("ocaslr_name", value);
				this.OnPropertyChanged("ocaslr_name");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_systemconfigurationid")]
		public System.Nullable<System.Guid> ocaslr_systemconfigurationId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("ocaslr_systemconfigurationid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfigurationId");
				this.SetAttributeValue("ocaslr_systemconfigurationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ocaslr_systemconfigurationId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_systemconfigurationid")]
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
				this.ocaslr_systemconfigurationId = value;
			}
		}
		
		/// <summary>
		/// Large text Value
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_textvalue")]
		public string Ocaslr_textvalue
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_textvalue");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_textvalue");
				this.SetAttributeValue("ocaslr_textvalue", value);
				this.OnPropertyChanged("Ocaslr_textvalue");
			}
		}
		
		/// <summary>
		/// The configuration value.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_value")]
		public string ocaslr_value
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_value");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_value");
				this.SetAttributeValue("ocaslr_value", value);
				this.OnPropertyChanged("ocaslr_value");
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
		/// Status of the System Configuration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.ocaslr_systemconfigurationState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.ocaslr_systemconfigurationState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.ocaslr_systemconfigurationState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the System Configuration
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
		/// 
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
		/// 
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
		/// 1:N ocaslr_systemconfiguration_ActivityPointers
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_ActivityPointers")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityPointer> ocaslr_systemconfiguration_ActivityPointers
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_systemconfiguration_ActivityPointers", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_ActivityPointers");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_systemconfiguration_ActivityPointers", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_ActivityPointers");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Annotations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> ocaslr_systemconfiguration_Annotations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_systemconfiguration_Annotations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Annotations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_systemconfiguration_Annotations", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Annotations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Appointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Appointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Appointment> ocaslr_systemconfiguration_Appointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_systemconfiguration_Appointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Appointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_systemconfiguration_Appointments", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Appointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> ocaslr_systemconfiguration_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_systemconfiguration_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_systemconfiguration_AsyncOperations", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> ocaslr_systemconfiguration_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_systemconfiguration_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_systemconfiguration_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_DuplicateBaseRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_DuplicateBaseRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_systemconfiguration_DuplicateBaseRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_systemconfiguration_DuplicateBaseRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_DuplicateBaseRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_systemconfiguration_DuplicateBaseRecord", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_DuplicateBaseRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_DuplicateMatchingRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_DuplicateMatchingRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_systemconfiguration_DuplicateMatchingRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_systemconfiguration_DuplicateMatchingRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_DuplicateMatchingRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_systemconfiguration_DuplicateMatchingRecord", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_DuplicateMatchingRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Emails
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Emails")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Email> ocaslr_systemconfiguration_Emails
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_systemconfiguration_Emails", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Emails");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_systemconfiguration_Emails", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Emails");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Faxes
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Faxes")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Fax> ocaslr_systemconfiguration_Faxes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_systemconfiguration_Faxes", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Faxes");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_systemconfiguration_Faxes", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Faxes");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Letters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Letters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Letter> ocaslr_systemconfiguration_Letters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_systemconfiguration_Letters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Letters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_systemconfiguration_Letters", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Letters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_PhoneCalls
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_PhoneCalls")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PhoneCall> ocaslr_systemconfiguration_PhoneCalls
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_systemconfiguration_PhoneCalls", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_PhoneCalls");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_systemconfiguration_PhoneCalls", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_PhoneCalls");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> ocaslr_systemconfiguration_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_systemconfiguration_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_systemconfiguration_ProcessSession", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_RecurringAppointmentMasters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_RecurringAppointmentMasters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster> ocaslr_systemconfiguration_RecurringAppointmentMasters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_systemconfiguration_RecurringAppointmentMasters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_RecurringAppointmentMasters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_systemconfiguration_RecurringAppointmentMasters", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_RecurringAppointmentMasters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_ServiceAppointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_ServiceAppointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ServiceAppointment> ocaslr_systemconfiguration_ServiceAppointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_systemconfiguration_ServiceAppointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_ServiceAppointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_systemconfiguration_ServiceAppointments", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_ServiceAppointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_SocialActivities
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_SocialActivities")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SocialActivity> ocaslr_systemconfiguration_SocialActivities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_systemconfiguration_SocialActivities", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_SocialActivities");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_systemconfiguration_SocialActivities", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_SocialActivities");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_Tasks
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_Tasks")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Task> ocaslr_systemconfiguration_Tasks
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_systemconfiguration_Tasks", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_Tasks");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_systemconfiguration_Tasks", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_Tasks");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_systemconfiguration_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_systemconfiguration_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> ocaslr_systemconfiguration_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_systemconfiguration_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_systemconfiguration_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_systemconfiguration_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("ocaslr_systemconfiguration_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_systemconfiguration_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_systemconfiguration_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_systemconfiguration_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_systemconfiguration_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_systemconfiguration_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_systemconfiguration_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_systemconfiguration_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_systemconfiguration_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_systemconfiguration_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_systemconfiguration_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_systemconfiguration_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_systemconfiguration_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_systemconfiguration_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_systemconfiguration_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_systemconfiguration_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_systemconfiguration_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_systemconfiguration_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_ocaslr_systemconfiguration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_ocaslr_systemconfiguration")]
		public Ocas.Domestic.Crm.Entities.Organization organization_ocaslr_systemconfiguration
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_ocaslr_systemconfiguration", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual ocaslr_systemconfiguration_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ocaslr_systemconfiguration_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}