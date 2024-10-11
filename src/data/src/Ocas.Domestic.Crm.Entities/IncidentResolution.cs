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
	public enum IncidentResolutionState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Open = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Completed = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Canceled = 2,
	}
	
	/// <summary>
	/// Special type of activity that includes description of the resolution, billing status, and the duration of the case.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("incidentresolution")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class IncidentResolution : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string ActivityId = "activityid";
			public const string Id = "activityid";
			public const string ActivityTypeCode = "activitytypecode";
			public const string ActualDurationMinutes = "actualdurationminutes";
			public const string ActualEnd = "actualend";
			public const string ActualStart = "actualstart";
			public const string Category = "category";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string Description = "description";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string IncidentId = "incidentid";
			public const string IsBilled = "isbilled";
			public const string IsRegularActivity = "isregularactivity";
			public const string IsWorkflowCreated = "isworkflowcreated";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string OverriddenCreatedOn = "overriddencreatedon";
			public const string OwnerId = "ownerid";
			public const string OwningBusinessUnit = "owningbusinessunit";
			public const string OwningTeam = "owningteam";
			public const string OwningUser = "owninguser";
			public const string ScheduledDurationMinutes = "scheduleddurationminutes";
			public const string ScheduledEnd = "scheduledend";
			public const string ScheduledStart = "scheduledstart";
			public const string ServiceId = "serviceid";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string Subcategory = "subcategory";
			public const string Subject = "subject";
			public const string TimeSpent = "timespent";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string activity_pointer_incident_resolution = "activity_pointer_incident_resolution";
			public const string business_unit_incident_resolution_activities = "business_unit_incident_resolution_activities";
			public const string Incident_IncidentResolutions = "Incident_IncidentResolutions";
			public const string lk_incidentresolution_createdby = "lk_incidentresolution_createdby";
			public const string lk_incidentresolution_createdonbehalfby = "lk_incidentresolution_createdonbehalfby";
			public const string lk_incidentresolution_modifiedby = "lk_incidentresolution_modifiedby";
			public const string lk_incidentresolution_modifiedonbehalfby = "lk_incidentresolution_modifiedonbehalfby";
			public const string service_incidentresolutions = "service_incidentresolutions";
			public const string team_incidentresolution = "team_incidentresolution";
			public const string user_incidentresolution = "user_incidentresolution";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public IncidentResolution() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "incidentresolution";
		
		public const string EntitySchemaName = "IncidentResolution";
		
		public const string PrimaryIdAttribute = "activityid";
		
		public const string PrimaryNameAttribute = "subject";
		
		public const int EntityTypeCode = 4206;
		
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
		/// Unique identifier of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
		public System.Nullable<System.Guid> ActivityId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("activityid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ActivityId");
				this.SetAttributeValue("activityid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ActivityId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
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
				this.ActivityId = value;
			}
		}
		
		/// <summary>
		/// Type of activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activitytypecode")]
		public string ActivityTypeCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("activitytypecode");
			}
		}
		
		/// <summary>
		/// Actual duration of the case resolution activity in minutes.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualdurationminutes")]
		public System.Nullable<int> ActualDurationMinutes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("actualdurationminutes");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ActualDurationMinutes");
				this.SetAttributeValue("actualdurationminutes", value);
				this.OnPropertyChanged("ActualDurationMinutes");
			}
		}
		
		/// <summary>
		/// Actual end time of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualend")]
		public System.Nullable<System.DateTime> ActualEnd
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("actualend");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ActualEnd");
				this.SetAttributeValue("actualend", value);
				this.OnPropertyChanged("ActualEnd");
			}
		}
		
		/// <summary>
		/// Actual start time of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualstart")]
		public System.Nullable<System.DateTime> ActualStart
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("actualstart");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ActualStart");
				this.SetAttributeValue("actualstart", value);
				this.OnPropertyChanged("ActualStart");
			}
		}
		
		/// <summary>
		/// Category for the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("category")]
		public string Category
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("category");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Category");
				this.SetAttributeValue("category", value);
				this.OnPropertyChanged("Category");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the case resolution activity.
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
		/// Date and time when the case resolution activity was created.
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
		/// Unique identifier of the delegate user who created the incidentresolution.
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
		/// Type additional information that describes the case resolution.
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
		/// Unique identifier of the data import or data migration that created this record.
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
		/// Unique identifier of the case.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("incidentid")]
		public Microsoft.Xrm.Sdk.EntityReference IncidentId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("incidentid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IncidentId");
				this.SetAttributeValue("incidentid", value);
				this.OnPropertyChanged("IncidentId");
			}
		}
		
		/// <summary>
		/// Information about whether the case resolution activity was billed as part of resolving a case.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isbilled")]
		public System.Nullable<bool> IsBilled
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isbilled");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsBilled");
				this.SetAttributeValue("isbilled", value);
				this.OnPropertyChanged("IsBilled");
			}
		}
		
		/// <summary>
		/// Information regarding whether the activity is a regular activity type or event type.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isregularactivity")]
		public System.Nullable<bool> IsRegularActivity
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isregularactivity");
			}
		}
		
		/// <summary>
		/// Information that specifies if the case resolution activity was created from a workflow rule.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isworkflowcreated")]
		public System.Nullable<bool> IsWorkflowCreated
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isworkflowcreated");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsWorkflowCreated");
				this.SetAttributeValue("isworkflowcreated", value);
				this.OnPropertyChanged("IsWorkflowCreated");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the case resolution activity.
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
		/// Date and time when the case resolution activity was last modified.
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
		/// Unique identifier of the delegate user who last modified the incidentresolution.
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
		/// Unique identifier of the user or team who owns the case resolution activity.
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
		/// Unique identifier of the business unit that owns the case resolution activity.
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
		/// Unique identifier of the team who owns the case resolution.
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
		/// Unique identifier of the user who owns the case resolution.
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
		/// Scheduled duration of the case resolution activity, specified in minutes.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduleddurationminutes")]
		public System.Nullable<int> ScheduledDurationMinutes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("scheduleddurationminutes");
			}
		}
		
		/// <summary>
		/// Scheduled end time of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduledend")]
		public System.Nullable<System.DateTime> ScheduledEnd
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("scheduledend");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ScheduledEnd");
				this.SetAttributeValue("scheduledend", value);
				this.OnPropertyChanged("ScheduledEnd");
			}
		}
		
		/// <summary>
		/// Scheduled start time of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduledstart")]
		public System.Nullable<System.DateTime> ScheduledStart
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("scheduledstart");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ScheduledStart");
				this.SetAttributeValue("scheduledstart", value);
				this.OnPropertyChanged("ScheduledStart");
			}
		}
		
		/// <summary>
		/// Unique identifier of the service with which the case resolution activity is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceid")]
		public Microsoft.Xrm.Sdk.EntityReference ServiceId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("serviceid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ServiceId");
				this.SetAttributeValue("serviceid", value);
				this.OnPropertyChanged("ServiceId");
			}
		}
		
		/// <summary>
		/// Shows whether the case resolution is open, completed, or canceled. By default, all case resolutions are completed and the status value can't be changed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.IncidentResolutionState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.IncidentResolutionState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.IncidentResolutionState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the case resolution activity.
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
		/// Subcategory of the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subcategory")]
		public string Subcategory
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("subcategory");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Subcategory");
				this.SetAttributeValue("subcategory", value);
				this.OnPropertyChanged("Subcategory");
			}
		}
		
		/// <summary>
		/// Subject associated with the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subject")]
		public string Subject
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("subject");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Subject");
				this.SetAttributeValue("subject", value);
				this.OnPropertyChanged("Subject");
			}
		}
		
		/// <summary>
		/// Time spent on the case resolution activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timespent")]
		public System.Nullable<int> TimeSpent
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("timespent");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TimeSpent");
				this.SetAttributeValue("timespent", value);
				this.OnPropertyChanged("TimeSpent");
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
		/// Version number of the case.
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
		/// 1:N incidentresolution_activity_parties
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("incidentresolution_activity_parties")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityParty> incidentresolution_activity_parties
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityParty>("incidentresolution_activity_parties", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("incidentresolution_activity_parties");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityParty>("incidentresolution_activity_parties", null, value);
				this.OnPropertyChanged("incidentresolution_activity_parties");
			}
		}
		
		/// <summary>
		/// 1:N IncidentResolution_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("IncidentResolution_Annotation")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> IncidentResolution_Annotation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("IncidentResolution_Annotation", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IncidentResolution_Annotation");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("IncidentResolution_Annotation", null, value);
				this.OnPropertyChanged("IncidentResolution_Annotation");
			}
		}
		
		/// <summary>
		/// 1:N IncidentResolution_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("IncidentResolution_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> IncidentResolution_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("IncidentResolution_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IncidentResolution_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("IncidentResolution_AsyncOperations", null, value);
				this.OnPropertyChanged("IncidentResolution_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N IncidentResolution_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("IncidentResolution_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> IncidentResolution_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("IncidentResolution_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IncidentResolution_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("IncidentResolution_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("IncidentResolution_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N userentityinstancedata_incidentresolution
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_incidentresolution")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_incidentresolution
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_incidentresolution", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_incidentresolution");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_incidentresolution", null, value);
				this.OnPropertyChanged("userentityinstancedata_incidentresolution");
			}
		}
		
		/// <summary>
		/// N:1 activity_pointer_incident_resolution
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("activity_pointer_incident_resolution")]
		public Ocas.Domestic.Crm.Entities.ActivityPointer activity_pointer_incident_resolution
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.ActivityPointer>("activity_pointer_incident_resolution", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("activity_pointer_incident_resolution");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.ActivityPointer>("activity_pointer_incident_resolution", null, value);
				this.OnPropertyChanged("activity_pointer_incident_resolution");
			}
		}
		
		/// <summary>
		/// N:1 business_unit_incident_resolution_activities
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("business_unit_incident_resolution_activities")]
		public Ocas.Domestic.Crm.Entities.BusinessUnit business_unit_incident_resolution_activities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.BusinessUnit>("business_unit_incident_resolution_activities", null);
			}
		}
		
		/// <summary>
		/// N:1 Incident_IncidentResolutions
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("incidentid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Incident_IncidentResolutions")]
		public Ocas.Domestic.Crm.Entities.Incident Incident_IncidentResolutions
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Incident>("Incident_IncidentResolutions", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Incident_IncidentResolutions");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Incident>("Incident_IncidentResolutions", null, value);
				this.OnPropertyChanged("Incident_IncidentResolutions");
			}
		}
		
		/// <summary>
		/// N:1 lk_incidentresolution_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_incidentresolution_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_incidentresolution_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_incidentresolution_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_incidentresolution_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_incidentresolution_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_incidentresolution_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_incidentresolution_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_incidentresolution_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_incidentresolution_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_incidentresolution_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_incidentresolution_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_incidentresolution_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_incidentresolution_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_incidentresolution_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_incidentresolution_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_incidentresolution_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 service_incidentresolutions
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("service_incidentresolutions")]
		public Ocas.Domestic.Crm.Entities.Service service_incidentresolutions
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Service>("service_incidentresolutions", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("service_incidentresolutions");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Service>("service_incidentresolutions", null, value);
				this.OnPropertyChanged("service_incidentresolutions");
			}
		}
		
		/// <summary>
		/// N:1 team_incidentresolution
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_incidentresolution")]
		public Ocas.Domestic.Crm.Entities.Team team_incidentresolution
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Team>("team_incidentresolution", null);
			}
		}
		
		/// <summary>
		/// N:1 user_incidentresolution
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_incidentresolution")]
		public Ocas.Domestic.Crm.Entities.SystemUser user_incidentresolution
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("user_incidentresolution", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual IncidentResolution_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((IncidentResolution_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}