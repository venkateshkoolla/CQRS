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
	public enum ocaslr_vouchercodegenerationState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// 
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("ocaslr_vouchercodegeneration")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class ocaslr_vouchercodegeneration : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
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
			public const string ocaslr_ApplicationCycle = "ocaslr_applicationcycle";
			public const string ocaslr_name = "ocaslr_name";
			public const string ocaslr_NumberofCodes = "ocaslr_numberofcodes";
			public const string ocaslr_vouchercodegenerationId = "ocaslr_vouchercodegenerationid";
			public const string Id = "ocaslr_vouchercodegenerationid";
			public const string OrganizationId = "organizationid";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string lk_ocaslr_vouchercodegeneration_createdby = "lk_ocaslr_vouchercodegeneration_createdby";
			public const string lk_ocaslr_vouchercodegeneration_createdonbehalfby = "lk_ocaslr_vouchercodegeneration_createdonbehalfby";
			public const string lk_ocaslr_vouchercodegeneration_modifiedby = "lk_ocaslr_vouchercodegeneration_modifiedby";
			public const string lk_ocaslr_vouchercodegeneration_modifiedonbehalfby = "lk_ocaslr_vouchercodegeneration_modifiedonbehalfby";
			public const string organization_ocaslr_vouchercodegeneration = "organization_ocaslr_vouchercodegeneration";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public ocaslr_vouchercodegeneration() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "ocaslr_vouchercodegeneration";
		
		public const string EntitySchemaName = "ocaslr_vouchercodegeneration";
		
		public const string PrimaryIdAttribute = "ocaslr_vouchercodegenerationid";
		
		public const string PrimaryNameAttribute = "ocaslr_name";
		
		public const int EntityTypeCode = 10198;
		
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
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_applicationcycle")]
		public System.Nullable<int> ocaslr_ApplicationCycle
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("ocaslr_applicationcycle");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_ApplicationCycle");
				this.SetAttributeValue("ocaslr_applicationcycle", value);
				this.OnPropertyChanged("ocaslr_ApplicationCycle");
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
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_numberofcodes")]
		public System.Nullable<int> ocaslr_NumberofCodes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("ocaslr_numberofcodes");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_NumberofCodes");
				this.SetAttributeValue("ocaslr_numberofcodes", value);
				this.OnPropertyChanged("ocaslr_NumberofCodes");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_vouchercodegenerationid")]
		public System.Nullable<System.Guid> ocaslr_vouchercodegenerationId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("ocaslr_vouchercodegenerationid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegenerationId");
				this.SetAttributeValue("ocaslr_vouchercodegenerationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ocaslr_vouchercodegenerationId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ocaslr_vouchercodegenerationid")]
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
				this.ocaslr_vouchercodegenerationId = value;
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
		/// Status of the Voucher Code Generation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.ocaslr_vouchercodegenerationState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.ocaslr_vouchercodegenerationState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.ocaslr_vouchercodegenerationState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the Voucher Code Generation
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
		/// 1:N ocaslr_vouchercodegeneration_ActivityPointers
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_ActivityPointers")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityPointer> ocaslr_vouchercodegeneration_ActivityPointers
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_vouchercodegeneration_ActivityPointers", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_ActivityPointers");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityPointer>("ocaslr_vouchercodegeneration_ActivityPointers", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_ActivityPointers");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Annotations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> ocaslr_vouchercodegeneration_Annotations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_vouchercodegeneration_Annotations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Annotations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("ocaslr_vouchercodegeneration_Annotations", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Annotations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Appointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Appointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Appointment> ocaslr_vouchercodegeneration_Appointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_vouchercodegeneration_Appointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Appointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Appointment>("ocaslr_vouchercodegeneration_Appointments", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Appointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> ocaslr_vouchercodegeneration_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_vouchercodegeneration_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("ocaslr_vouchercodegeneration_AsyncOperations", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> ocaslr_vouchercodegeneration_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_vouchercodegeneration_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("ocaslr_vouchercodegeneration_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_connections1")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Connection> ocaslr_vouchercodegeneration_connections1
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Connection>("ocaslr_vouchercodegeneration_connections1", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_connections1");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Connection>("ocaslr_vouchercodegeneration_connections1", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_connections1");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_connections2")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Connection> ocaslr_vouchercodegeneration_connections2
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Connection>("ocaslr_vouchercodegeneration_connections2", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_connections2");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Connection>("ocaslr_vouchercodegeneration_connections2", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_connections2");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_DuplicateBaseRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_DuplicateBaseRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_vouchercodegeneration_DuplicateBaseRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_vouchercodegeneration_DuplicateBaseRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_DuplicateBaseRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_vouchercodegeneration_DuplicateBaseRecord", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_DuplicateBaseRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_DuplicateMatchingRecord
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_DuplicateMatchingRecord")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.DuplicateRecord> ocaslr_vouchercodegeneration_DuplicateMatchingRecord
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_vouchercodegeneration_DuplicateMatchingRecord", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_DuplicateMatchingRecord");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.DuplicateRecord>("ocaslr_vouchercodegeneration_DuplicateMatchingRecord", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_DuplicateMatchingRecord");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Emails
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Emails")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Email> ocaslr_vouchercodegeneration_Emails
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_vouchercodegeneration_Emails", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Emails");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Email>("ocaslr_vouchercodegeneration_Emails", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Emails");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Faxes
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Faxes")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Fax> ocaslr_vouchercodegeneration_Faxes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_vouchercodegeneration_Faxes", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Faxes");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Fax>("ocaslr_vouchercodegeneration_Faxes", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Faxes");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Letters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Letters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Letter> ocaslr_vouchercodegeneration_Letters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_vouchercodegeneration_Letters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Letters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Letter>("ocaslr_vouchercodegeneration_Letters", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Letters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_PhoneCalls
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_PhoneCalls")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PhoneCall> ocaslr_vouchercodegeneration_PhoneCalls
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_vouchercodegeneration_PhoneCalls", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_PhoneCalls");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PhoneCall>("ocaslr_vouchercodegeneration_PhoneCalls", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_PhoneCalls");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess> ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.PrincipalObjectAttributeAccess>("ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_PrincipalObjectAttributeAccesses");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_ProcessSession
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_ProcessSession")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ProcessSession> ocaslr_vouchercodegeneration_ProcessSession
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_vouchercodegeneration_ProcessSession", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_ProcessSession");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ProcessSession>("ocaslr_vouchercodegeneration_ProcessSession", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_ProcessSession");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_RecurringAppointmentMasters
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_RecurringAppointmentMasters")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster> ocaslr_vouchercodegeneration_RecurringAppointmentMasters
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_vouchercodegeneration_RecurringAppointmentMasters", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_RecurringAppointmentMasters");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.RecurringAppointmentMaster>("ocaslr_vouchercodegeneration_RecurringAppointmentMasters", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_RecurringAppointmentMasters");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_ServiceAppointments
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_ServiceAppointments")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ServiceAppointment> ocaslr_vouchercodegeneration_ServiceAppointments
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_vouchercodegeneration_ServiceAppointments", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_ServiceAppointments");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ServiceAppointment>("ocaslr_vouchercodegeneration_ServiceAppointments", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_ServiceAppointments");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_SocialActivities
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_SocialActivities")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SocialActivity> ocaslr_vouchercodegeneration_SocialActivities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_vouchercodegeneration_SocialActivities", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_SocialActivities");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialActivity>("ocaslr_vouchercodegeneration_SocialActivities", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_SocialActivities");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_Tasks
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_Tasks")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Task> ocaslr_vouchercodegeneration_Tasks
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_vouchercodegeneration_Tasks", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_Tasks");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Task>("ocaslr_vouchercodegeneration_Tasks", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_Tasks");
			}
		}
		
		/// <summary>
		/// 1:N ocaslr_vouchercodegeneration_UserEntityInstanceDatas
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("ocaslr_vouchercodegeneration_UserEntityInstanceDatas")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> ocaslr_vouchercodegeneration_UserEntityInstanceDatas
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_vouchercodegeneration_UserEntityInstanceDatas", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ocaslr_vouchercodegeneration_UserEntityInstanceDatas");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("ocaslr_vouchercodegeneration_UserEntityInstanceDatas", null, value);
				this.OnPropertyChanged("ocaslr_vouchercodegeneration_UserEntityInstanceDatas");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_vouchercodegeneration_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_vouchercodegeneration_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_vouchercodegeneration_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_vouchercodegeneration_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_vouchercodegeneration_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_vouchercodegeneration_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_vouchercodegeneration_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_vouchercodegeneration_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_vouchercodegeneration_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_vouchercodegeneration_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_vouchercodegeneration_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_ocaslr_vouchercodegeneration_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_ocaslr_vouchercodegeneration_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_ocaslr_vouchercodegeneration_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_ocaslr_vouchercodegeneration_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_ocaslr_vouchercodegeneration_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_ocaslr_vouchercodegeneration_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_ocaslr_vouchercodegeneration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_ocaslr_vouchercodegeneration")]
		public Ocas.Domestic.Crm.Entities.Organization organization_ocaslr_vouchercodegeneration
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_ocaslr_vouchercodegeneration", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual ocaslr_vouchercodegeneration_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ocaslr_vouchercodegeneration_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}