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
	public enum ocaslr_transmissionerrormessageState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// ^Transmission Error Message
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("ocaslr_transmissionerrormessage")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class ocaslr_transmissionerrormessage : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
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
			public const string ocaslr_category = "ocaslr_category";
			public const string ocaslr_code = "ocaslr_code";
			public const string ocaslr_description = "ocaslr_description";
			public const string ocaslr_FrenchDescription = "ocaslr_frenchdescription";
			public const string ocaslr_name = "ocaslr_name";
			public const string ocaslr_transmissionerrormessageId = "ocaslr_transmissionerrormessageid";
			public const string Id = "ocaslr_transmissionerrormessageid";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_ocaslr_transmissionerrormessage_createdby = "lk_ocaslr_transmissionerrormessage_createdby";
			public const string lk_ocaslr_transmissionerrormessage_createdonbehalfby = "lk_ocaslr_transmissionerrormessage_createdonbehalfby";
			public const string lk_ocaslr_transmissionerrormessage_modifiedby = "lk_ocaslr_transmissionerrormessage_modifiedby";
			public const string lk_ocaslr_transmissionerrormessage_modifiedonbehalfby = "lk_ocaslr_transmissionerrormessage_modifiedonbehalfby";
			public const string organization_ocaslr_transmissionerrormessage = "organization_ocaslr_transmissionerrormessage";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public ocaslr_transmissionerrormessage() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "ocaslr_transmissionerrormessage";
		
		public const string EntitySchemaName = "ocaslr_transmissionerrormessage";
		
		public const string PrimaryIdAttribute = "ocaslr_transmissionerrormessageid";
		
		public const string PrimaryNameAttribute = "ocaslr_name";
		
		public const int EntityTypeCode = 10133;
		
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
		/// Error Category
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_category")]
		public Microsoft.Xrm.Sdk.OptionSetValue ocaslr_category
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("ocaslr_category");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_category");
				this.SetAttributeValue("ocaslr_category", value);
				this.OnPropertyChanged("ocaslr_category");
			}
		}
		
		/// <summary>
		/// Error Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_code")]
		public string ocaslr_code
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_code");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_code");
				this.SetAttributeValue("ocaslr_code", value);
				this.OnPropertyChanged("ocaslr_code");
			}
		}
		
		/// <summary>
		/// Error Message
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_description")]
		public string ocaslr_description
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_description");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_description");
				this.SetAttributeValue("ocaslr_description", value);
				this.OnPropertyChanged("ocaslr_description");
			}
		}
		
		/// <summary>
		/// French Error Message
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_frenchdescription")]
		public string ocaslr_FrenchDescription
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_frenchdescription");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_FrenchDescription");
				this.SetAttributeValue("ocaslr_frenchdescription", value);
				this.OnPropertyChanged("ocaslr_FrenchDescription");
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
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_transmissionerrormessageid")]
		public System.Nullable<System.Guid> ocaslr_transmissionerrormessageId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("ocaslr_transmissionerrormessageid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessageId");
				this.SetAttributeValue("ocaslr_transmissionerrormessageid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ocaslr_transmissionerrormessageId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_transmissionerrormessageid")]
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
				this.ocaslr_transmissionerrormessageId = value;
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
		/// Status of the Transmission Error Message
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.ocaslr_transmissionerrormessageState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.ocaslr_transmissionerrormessageState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.ocaslr_transmissionerrormessageState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the Transmission Error Message
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
		/// 1:N ocaslr_transmissionerrormessage_ActivityPointers
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_ActivityPointers")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityPointer> ocaslr_transmissionerrormessage_ActivityPointers
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_transmissionerrormessage_ActivityPointers", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_ActivityPointers");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_transmissionerrormessage_ActivityPointers", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_ActivityPointers");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Annotations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> ocaslr_transmissionerrormessage_Annotations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_transmissionerrormessage_Annotations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Annotations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_transmissionerrormessage_Annotations", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Annotations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Appointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Appointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Appointment> ocaslr_transmissionerrormessage_Appointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_transmissionerrormessage_Appointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Appointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_transmissionerrormessage_Appointments", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Appointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> ocaslr_transmissionerrormessage_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_transmissionerrormessage_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_transmissionerrormessage_AsyncOperations", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> ocaslr_transmissionerrormessage_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_transmissionerrormessage_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_transmissionerrormessage_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_detail
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_detail")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ocaslr_transmissiondetail> ocaslr_transmissionerrormessage_detail
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_transmissiondetail>("ocaslr_transmissionerrormessage_detail", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_detail");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_transmissiondetail>("ocaslr_transmissionerrormessage_detail", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_detail");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_DuplicateBaseRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_DuplicateBaseRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_transmissionerrormessage_DuplicateBaseRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_transmissionerrormessage_DuplicateBaseRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_DuplicateBaseRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_transmissionerrormessage_DuplicateBaseRecord", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_DuplicateBaseRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_DuplicateMatchingRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_DuplicateMatchingRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_transmissionerrormessage_DuplicateMatchingRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_transmissionerrormessage_DuplicateMatchingRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_DuplicateMatchingRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_transmissionerrormessage_DuplicateMatchingRecord", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_DuplicateMatchingRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Emails
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Emails")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Email> ocaslr_transmissionerrormessage_Emails
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_transmissionerrormessage_Emails", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Emails");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_transmissionerrormessage_Emails", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Emails");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Faxes
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Faxes")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Fax> ocaslr_transmissionerrormessage_Faxes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_transmissionerrormessage_Faxes", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Faxes");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_transmissionerrormessage_Faxes", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Faxes");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Letters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Letters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Letter> ocaslr_transmissionerrormessage_Letters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_transmissionerrormessage_Letters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Letters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_transmissionerrormessage_Letters", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Letters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_PhoneCalls
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_PhoneCalls")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PhoneCall> ocaslr_transmissionerrormessage_PhoneCalls
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_transmissionerrormessage_PhoneCalls", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_PhoneCalls");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_transmissionerrormessage_PhoneCalls", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_PhoneCalls");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> ocaslr_transmissionerrormessage_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_transmissionerrormessage_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_transmissionerrormessage_ProcessSession", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_RecurringAppointmentMasters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_RecurringAppointmentMasters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster> ocaslr_transmissionerrormessage_RecurringAppointmentMasters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_transmissionerrormessage_RecurringAppointmentMasters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_RecurringAppointmentMasters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_transmissionerrormessage_RecurringAppointmentMasters", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_RecurringAppointmentMasters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_ServiceAppointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_ServiceAppointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ServiceAppointment> ocaslr_transmissionerrormessage_ServiceAppointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_transmissionerrormessage_ServiceAppointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_ServiceAppointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_transmissionerrormessage_ServiceAppointments", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_ServiceAppointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_SocialActivities
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_SocialActivities")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SocialActivity> ocaslr_transmissionerrormessage_SocialActivities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_transmissionerrormessage_SocialActivities", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_SocialActivities");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_transmissionerrormessage_SocialActivities", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_SocialActivities");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_Tasks
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_Tasks")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Task> ocaslr_transmissionerrormessage_Tasks
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_transmissionerrormessage_Tasks", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_Tasks");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_transmissionerrormessage_Tasks", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_Tasks");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_transmissionerrormessage_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_transmissionerrormessage_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> ocaslr_transmissionerrormessage_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_transmissionerrormessage_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_transmissionerrormessage_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_transmissionerrormessage_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("ocaslr_transmissionerrormessage_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_transmissionerrormessage_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_transmissionerrormessage_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_transmissionerrormessage_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_transmissionerrormessage_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_transmissionerrormessage_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_transmissionerrormessage_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_transmissionerrormessage_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_transmissionerrormessage_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_transmissionerrormessage_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_transmissionerrormessage_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_transmissionerrormessage_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_transmissionerrormessage_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_transmissionerrormessage_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_transmissionerrormessage_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_transmissionerrormessage_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_transmissionerrormessage_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_transmissionerrormessage_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_ocaslr_transmissionerrormessage
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_ocaslr_transmissionerrormessage")]
		public Ocas.Domestic.Crm.Entities.Organization organization_ocaslr_transmissionerrormessage
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_ocaslr_transmissionerrormessage", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_category")]
		public virtual ocaslr_transmissionerrormessage_ocaslr_category? ocaslr_categoryEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ocaslr_transmissionerrormessage_ocaslr_category?)(EntityOptionSetEnum.GetEnum(this, "ocaslr_category")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				ocaslr_category = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual ocaslr_transmissionerrormessage_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ocaslr_transmissionerrormessage_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}