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
	public enum ocaslr_credentialState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// The credential received upon completion of the program
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("ocaslr_credential")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class ocaslr_credential : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
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
			public const string ocaslr_code = "ocaslr_code";
			public const string ocaslr_coltranecode = "ocaslr_coltranecode";
			public const string ocaslr_credentialId = "ocaslr_credentialid";
			public const string Id = "ocaslr_credentialid";
			public const string ocaslr_dwcode = "ocaslr_dwcode";
			public const string ocaslr_englishdescription = "ocaslr_englishdescription";
			public const string ocaslr_frenchdescription = "ocaslr_frenchdescription";
			public const string ocaslr_name = "ocaslr_name";
			public const string ocaslr_sortorder = "ocaslr_sortorder";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_ocaslr_credential_createdby = "lk_ocaslr_credential_createdby";
			public const string lk_ocaslr_credential_createdonbehalfby = "lk_ocaslr_credential_createdonbehalfby";
			public const string lk_ocaslr_credential_modifiedby = "lk_ocaslr_credential_modifiedby";
			public const string lk_ocaslr_credential_modifiedonbehalfby = "lk_ocaslr_credential_modifiedonbehalfby";
			public const string organization_ocaslr_credential = "organization_ocaslr_credential";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public ocaslr_credential() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "ocaslr_credential";
		
		public const string EntitySchemaName = "ocaslr_credential";
		
		public const string PrimaryIdAttribute = "ocaslr_credentialid";
		
		public const string PrimaryNameAttribute = "ocaslr_name";
		
		public const int EntityTypeCode = 10040;
		
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
		/// The coltran cod for the credential.
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
		/// Unique Identifier for the Coltrane Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_coltranecode")]
		public string ocaslr_coltranecode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_coltranecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_coltranecode");
				this.SetAttributeValue("ocaslr_coltranecode", value);
				this.OnPropertyChanged("ocaslr_coltranecode");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_credentialid")]
		public System.Nullable<System.Guid> ocaslr_credentialId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("ocaslr_credentialid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credentialId");
				this.SetAttributeValue("ocaslr_credentialid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ocaslr_credentialId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_credentialid")]
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
				this.ocaslr_credentialId = value;
			}
		}
		
		/// <summary>
		/// Unique Identifier for the Data Warehouse Code
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_dwcode")]
		public string ocaslr_dwcode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_dwcode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_dwcode");
				this.SetAttributeValue("ocaslr_dwcode", value);
				this.OnPropertyChanged("ocaslr_dwcode");
			}
		}
		
		/// <summary>
		/// English Description for the Credential
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_englishdescription")]
		public string ocaslr_englishdescription
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_englishdescription");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_englishdescription");
				this.SetAttributeValue("ocaslr_englishdescription", value);
				this.OnPropertyChanged("ocaslr_englishdescription");
			}
		}
		
		/// <summary>
		/// French Description of the Credential
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_frenchdescription")]
		public string ocaslr_frenchdescription
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("ocaslr_frenchdescription");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_frenchdescription");
				this.SetAttributeValue("ocaslr_frenchdescription", value);
				this.OnPropertyChanged("ocaslr_frenchdescription");
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
		/// Sort Order of the Credential
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_sortorder")]
		public System.Nullable<int> ocaslr_sortorder
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("ocaslr_sortorder");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_sortorder");
				this.SetAttributeValue("ocaslr_sortorder", value);
				this.OnPropertyChanged("ocaslr_sortorder");
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
		/// Status of the Credential
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.ocaslr_credentialState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.ocaslr_credentialState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.ocaslr_credentialState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the Credential
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
		/// 1:N ocaslr_credential_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> ocaslr_credential_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_credential_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_credential_AsyncOperations", null, value);
				this.OnPropertyChanged("ocaslr_credential_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> ocaslr_credential_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_credential_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_credential_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("ocaslr_credential_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_DuplicateBaseRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_DuplicateBaseRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_credential_DuplicateBaseRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_credential_DuplicateBaseRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_DuplicateBaseRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_credential_DuplicateBaseRecord", null, value);
				this.OnPropertyChanged("ocaslr_credential_DuplicateBaseRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_DuplicateMatchingRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_DuplicateMatchingRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_credential_DuplicateMatchingRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_credential_DuplicateMatchingRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_DuplicateMatchingRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_credential_DuplicateMatchingRecord", null, value);
				this.OnPropertyChanged("ocaslr_credential_DuplicateMatchingRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_education
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_education")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ocaslr_education> ocaslr_credential_education
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_education>("ocaslr_credential_education", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_education");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_education>("ocaslr_credential_education", null, value);
				this.OnPropertyChanged("ocaslr_credential_education");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_ocaslr_program
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_ocaslr_program")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ocaslr_program> ocaslr_credential_ocaslr_program
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_program>("ocaslr_credential_ocaslr_program", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_ocaslr_program");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_program>("ocaslr_credential_ocaslr_program", null, value);
				this.OnPropertyChanged("ocaslr_credential_ocaslr_program");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> ocaslr_credential_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_credential_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_credential_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("ocaslr_credential_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> ocaslr_credential_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_credential_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_credential_ProcessSession", null, value);
				this.OnPropertyChanged("ocaslr_credential_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_supportingdocument
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_supportingdocument")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ocaslr_supportingdocument> ocaslr_credential_supportingdocument
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_supportingdocument>("ocaslr_credential_supportingdocument", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_supportingdocument");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ocaslr_supportingdocument>("ocaslr_credential_supportingdocument", null, value);
				this.OnPropertyChanged("ocaslr_credential_supportingdocument");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_credential_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_credential_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> ocaslr_credential_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_credential_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_credential_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_credential_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("ocaslr_credential_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_credential_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_credential_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_credential_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_credential_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_credential_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_credential_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_credential_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_credential_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_credential_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_credential_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_credential_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_credential_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_credential_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_credential_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_credential_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_credential_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_credential_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_ocaslr_credential
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_ocaslr_credential")]
		public Ocas.Domestic.Crm.Entities.Organization organization_ocaslr_credential
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_ocaslr_credential", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual ocaslr_credential_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ocaslr_credential_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}