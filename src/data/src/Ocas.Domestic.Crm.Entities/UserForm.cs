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
	/// User-owned dashboards.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("userform")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class UserForm : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string Description = "description";
			public const string FormXml = "formxml";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string Name = "name";
			public const string ObjectTypeCode = "objecttypecode";
			public const string OwnerId = "ownerid";
			public const string OwningBusinessUnit = "owningbusinessunit";
			public const string OwningTeam = "owningteam";
			public const string OwningUser = "owninguser";
			public const string Type = "type";
			public const string UserFormId = "userformid";
			public const string Id = "userformid";
			public const string VersionNumber = "versionnumber";
			public const string business_unit_userform = "business_unit_userform";
			public const string lk_userform_createdby = "lk_userform_createdby";
			public const string lk_userform_modifiedby = "lk_userform_modifiedby";
			public const string lk_userformbase_createdonbehalfby = "lk_userformbase_createdonbehalfby";
			public const string lk_userformbase_modifiedonbehalfby = "lk_userformbase_modifiedonbehalfby";
			public const string team_userform = "team_userform";
			public const string user_userform = "user_userform";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public UserForm() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "userform";
		
		public const string EntitySchemaName = "UserForm";
		
		public const string PrimaryIdAttribute = "userformid";
		
		public const string PrimaryNameAttribute = "name";
		
		public const int EntityTypeCode = 1031;
		
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
		/// Shows who created the record.
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
		/// Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics CRM options.
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
		/// Shows who created the record on behalf of another user.
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
		/// Type additional information to describe the form or dashboard, such as the filter criteria or intended audience.
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
		/// Shows the XML representation of the layout of the form or dashboard.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("formxml")]
		public string FormXml
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("formxml");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("FormXml");
				this.SetAttributeValue("formxml", value);
				this.OnPropertyChanged("FormXml");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record.
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
		/// Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics CRM options.
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
		/// Shows who created the record on behalf of another user.
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
		/// Type a descriptive name for the form or dashboard.
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
		/// Shows the record type (entity) code that the form applies to.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objecttypecode")]
		public string ObjectTypeCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("objecttypecode");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ObjectTypeCode");
				this.SetAttributeValue("objecttypecode", value);
				this.OnPropertyChanged("ObjectTypeCode");
			}
		}
		
		/// <summary>
		/// Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
		public Microsoft.Xrm.Sdk.EntityReference OwnerId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OwnerId");
				this.SetAttributeValue("ownerid", value);
				this.OnPropertyChanged("OwnerId");
			}
		}
		
		/// <summary>
		/// Shows the business unit that the record owner belongs to.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		public Microsoft.Xrm.Sdk.EntityReference OwningBusinessUnit
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningbusinessunit");
			}
		}
		
		/// <summary>
		/// Unique identifier of the team who owns the dashboard.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		public Microsoft.Xrm.Sdk.EntityReference OwningTeam
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who owns the dashboard.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		public Microsoft.Xrm.Sdk.EntityReference OwningUser
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Select the form type.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("type")]
		public Microsoft.Xrm.Sdk.OptionSetValue Type
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("type");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Type");
				this.SetAttributeValue("type", value);
				this.OnPropertyChanged("Type");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user dashboard.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userformid")]
		public System.Nullable<System.Guid> UserFormId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("userformid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UserFormId");
				this.SetAttributeValue("userformid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("UserFormId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("userformid")]
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
				this.UserFormId = value;
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
		/// 1:N socialinsightsconfiguration_userform
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("socialinsightsconfiguration_userform")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SocialInsightsConfiguration> socialinsightsconfiguration_userform
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialInsightsConfiguration>("socialinsightsconfiguration_userform", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("socialinsightsconfiguration_userform");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SocialInsightsConfiguration>("socialinsightsconfiguration_userform", null, value);
				this.OnPropertyChanged("socialinsightsconfiguration_userform");
			}
		}
		
		/// <summary>
		/// 1:N userentityinstancedata_userform
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_userform")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_userform
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_userform", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_userform");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_userform", null, value);
				this.OnPropertyChanged("userentityinstancedata_userform");
			}
		}
		
		/// <summary>
		/// 1:N UserForm_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("UserForm_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> UserForm_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("UserForm_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UserForm_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("UserForm_AsyncOperations", null, value);
				this.OnPropertyChanged("UserForm_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N UserForm_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("UserForm_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> UserForm_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("UserForm_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("UserForm_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("UserForm_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("UserForm_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// N:1 business_unit_userform
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("business_unit_userform")]
		public Ocas.Domestic.Crm.Entities.BusinessUnit business_unit_userform
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.BusinessUnit>("business_unit_userform", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_userform_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_userform_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_userform_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userform_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_userform_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_userform_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_userform_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userform_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_userformbase_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_userformbase_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_userformbase_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userformbase_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_userformbase_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userformbase_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_userformbase_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_userformbase_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_userformbase_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_userformbase_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userformbase_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_userformbase_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_userformbase_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_userformbase_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 team_userform
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_userform")]
		public Ocas.Domestic.Crm.Entities.Team team_userform
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Team>("team_userform", null);
			}
		}
		
		/// <summary>
		/// N:1 user_userform
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_userform")]
		public Ocas.Domestic.Crm.Entities.SystemUser user_userform
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("user_userform", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("type")]
		public virtual UserForm_Type? TypeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((UserForm_Type?)(EntityOptionSetEnum.GetEnum(this, "type")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				Type = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}