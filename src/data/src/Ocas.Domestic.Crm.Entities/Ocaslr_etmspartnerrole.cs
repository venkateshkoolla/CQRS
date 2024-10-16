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
	public enum Ocaslr_etmspartnerroleState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// This entity will be a lookup to the list of valid partner roles partners may have in the eTMS exchange (Consumer, Supplier).
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("ocaslr_etmspartnerrole")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class Ocaslr_etmspartnerrole : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
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
			public const string Ocaslr_Code = "ocaslr_code";
			public const string Ocaslr_ColtraneCode = "ocaslr_coltranecode";
			public const string Ocaslr_DWCode = "ocaslr_dwcode";
			public const string Ocaslr_EnglishDescription = "ocaslr_englishdescription";
			public const string Ocaslr_etmspartnerroleId = "ocaslr_etmspartnerroleid";
			public const string Id = "ocaslr_etmspartnerroleid";
			public const string Ocaslr_FrenchDescription = "ocaslr_frenchdescription";
			public const string Ocaslr_name = "ocaslr_name";
			public const string Ocaslr_SortOrder = "ocaslr_sortorder";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_ocaslr_etmspartnerrole_createdby = "lk_ocaslr_etmspartnerrole_createdby";
			public const string lk_ocaslr_etmspartnerrole_createdonbehalfby = "lk_ocaslr_etmspartnerrole_createdonbehalfby";
			public const string lk_ocaslr_etmspartnerrole_modifiedby = "lk_ocaslr_etmspartnerrole_modifiedby";
			public const string lk_ocaslr_etmspartnerrole_modifiedonbehalfby = "lk_ocaslr_etmspartnerrole_modifiedonbehalfby";
			public const string organization_ocaslr_etmspartnerrole = "organization_ocaslr_etmspartnerrole";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public Ocaslr_etmspartnerrole() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "ocaslr_etmspartnerrole";
		
		public const string EntitySchemaName = "Ocaslr_etmspartnerrole";
		
		public const string PrimaryIdAttribute = "ocaslr_etmspartnerroleid";
		
		public const string PrimaryNameAttribute = "ocaslr_name";
		
		public const int EntityTypeCode = 10149;
		
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
		/// Unique Identifier for the Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_code")]
		public string Ocaslr_Code
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_code");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_Code");
				this.SetAttributeValue("ocaslr_code", value);
				this.OnPropertyChanged("Ocaslr_Code");
			}
		}
		
		/// <summary>
		/// Unique Identifier for the Coltrane Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_coltranecode")]
		public string Ocaslr_ColtraneCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_coltranecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_ColtraneCode");
				this.SetAttributeValue("ocaslr_coltranecode", value);
				this.OnPropertyChanged("Ocaslr_ColtraneCode");
			}
		}
		
		/// <summary>
		/// Unique Identifier for the Data Warehouse Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_dwcode")]
		public string Ocaslr_DWCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_dwcode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_DWCode");
				this.SetAttributeValue("ocaslr_dwcode", value);
				this.OnPropertyChanged("Ocaslr_DWCode");
			}
		}
		
		/// <summary>
		/// English Description for the eTMS Partner Role
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_englishdescription")]
		public string Ocaslr_EnglishDescription
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_englishdescription");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_EnglishDescription");
				this.SetAttributeValue("ocaslr_englishdescription", value);
				this.OnPropertyChanged("Ocaslr_EnglishDescription");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_etmspartnerroleid")]
		public System.Nullable<System.Guid> Ocaslr_etmspartnerroleId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("ocaslr_etmspartnerroleid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_etmspartnerroleId");
				this.SetAttributeValue("ocaslr_etmspartnerroleid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("Ocaslr_etmspartnerroleId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_etmspartnerroleid")]
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
				this.Ocaslr_etmspartnerroleId = value;
			}
		}
		
		/// <summary>
		/// French Description for the eTMS Partner Role
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_frenchdescription")]
		public string Ocaslr_FrenchDescription
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_frenchdescription");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_FrenchDescription");
				this.SetAttributeValue("ocaslr_frenchdescription", value);
				this.OnPropertyChanged("Ocaslr_FrenchDescription");
			}
		}
		
		/// <summary>
		/// *The name of the custom entity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_name")]
		public string Ocaslr_name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_name");
				this.SetAttributeValue("ocaslr_name", value);
				this.OnPropertyChanged("Ocaslr_name");
			}
		}
		
		/// <summary>
		/// Sort Order for the eTMS Partner Role
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_sortorder")]
		public System.Nullable<int> Ocaslr_SortOrder
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("ocaslr_sortorder");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Ocaslr_SortOrder");
				this.SetAttributeValue("ocaslr_sortorder", value);
				this.OnPropertyChanged("Ocaslr_SortOrder");
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
		/// Status of the eTMS Partner Role
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.Ocaslr_etmspartnerroleState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.Ocaslr_etmspartnerroleState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.Ocaslr_etmspartnerroleState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the eTMS Partner Role
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
		/// 1:N ocaslr_etmspartnerrole_ActivityPointers
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_ActivityPointers")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityPointer> ocaslr_etmspartnerrole_ActivityPointers
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_etmspartnerrole_ActivityPointers", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_ActivityPointers");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_etmspartnerrole_ActivityPointers", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_ActivityPointers");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Annotations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> ocaslr_etmspartnerrole_Annotations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_etmspartnerrole_Annotations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Annotations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_etmspartnerrole_Annotations", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Annotations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Appointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Appointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Appointment> ocaslr_etmspartnerrole_Appointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_etmspartnerrole_Appointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Appointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_etmspartnerrole_Appointments", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Appointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> ocaslr_etmspartnerrole_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_etmspartnerrole_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_etmspartnerrole_AsyncOperations", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> ocaslr_etmspartnerrole_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_etmspartnerrole_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_etmspartnerrole_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_DuplicateBaseRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_DuplicateBaseRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_etmspartnerrole_DuplicateBaseRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_etmspartnerrole_DuplicateBaseRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_DuplicateBaseRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_etmspartnerrole_DuplicateBaseRecord", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_DuplicateBaseRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_DuplicateMatchingRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_DuplicateMatchingRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_etmspartnerrole_DuplicateMatchingRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_etmspartnerrole_DuplicateMatchingRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_DuplicateMatchingRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_etmspartnerrole_DuplicateMatchingRecord", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_DuplicateMatchingRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Emails
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Emails")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Email> ocaslr_etmspartnerrole_Emails
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_etmspartnerrole_Emails", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Emails");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_etmspartnerrole_Emails", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Emails");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Faxes
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Faxes")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Fax> ocaslr_etmspartnerrole_Faxes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_etmspartnerrole_Faxes", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Faxes");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_etmspartnerrole_Faxes", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Faxes");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Letters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Letters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Letter> ocaslr_etmspartnerrole_Letters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_etmspartnerrole_Letters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Letters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_etmspartnerrole_Letters", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Letters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_PhoneCalls
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_PhoneCalls")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PhoneCall> ocaslr_etmspartnerrole_PhoneCalls
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_etmspartnerrole_PhoneCalls", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_PhoneCalls");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_etmspartnerrole_PhoneCalls", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_PhoneCalls");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> ocaslr_etmspartnerrole_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_etmspartnerrole_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_etmspartnerrole_ProcessSession", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_RecurringAppointmentMasters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_RecurringAppointmentMasters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster> ocaslr_etmspartnerrole_RecurringAppointmentMasters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_etmspartnerrole_RecurringAppointmentMasters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_RecurringAppointmentMasters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_etmspartnerrole_RecurringAppointmentMasters", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_RecurringAppointmentMasters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_ServiceAppointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_ServiceAppointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ServiceAppointment> ocaslr_etmspartnerrole_ServiceAppointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_etmspartnerrole_ServiceAppointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_ServiceAppointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_etmspartnerrole_ServiceAppointments", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_ServiceAppointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_SocialActivities
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_SocialActivities")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SocialActivity> ocaslr_etmspartnerrole_SocialActivities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_etmspartnerrole_SocialActivities", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_SocialActivities");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_etmspartnerrole_SocialActivities", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_SocialActivities");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_Tasks
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_Tasks")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Task> ocaslr_etmspartnerrole_Tasks
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_etmspartnerrole_Tasks", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_Tasks");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_etmspartnerrole_Tasks", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_Tasks");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_etmspartnerrole_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_etmspartnerrole_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> ocaslr_etmspartnerrole_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_etmspartnerrole_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_etmspartnerrole_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_etmspartnerrole_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("ocaslr_etmspartnerrole_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// N:N ocaslr_account_etmsrolemembership
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_account_etmsrolemembership")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Account> ocaslr_account_etmsrolemembership
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Account>("ocaslr_account_etmsrolemembership", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_account_etmsrolemembership");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Account>("ocaslr_account_etmsrolemembership", null, value);
				this.OnPropertyChanged("ocaslr_account_etmsrolemembership");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_etmspartnerrole_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_etmspartnerrole_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_etmspartnerrole_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_etmspartnerrole_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_etmspartnerrole_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_etmspartnerrole_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_etmspartnerrole_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_etmspartnerrole_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_etmspartnerrole_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_etmspartnerrole_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_etmspartnerrole_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_etmspartnerrole_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_etmspartnerrole_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_etmspartnerrole_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_etmspartnerrole_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_etmspartnerrole_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_etmspartnerrole_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_ocaslr_etmspartnerrole
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_ocaslr_etmspartnerrole")]
		public Ocas.Domestic.Crm.Entities.Organization organization_ocaslr_etmspartnerrole
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_ocaslr_etmspartnerrole", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual Ocaslr_etmspartnerrole_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((Ocaslr_etmspartnerrole_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}