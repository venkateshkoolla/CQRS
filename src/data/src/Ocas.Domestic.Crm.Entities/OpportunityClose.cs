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
	public enum OpportunityCloseState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Open = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Completed = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Canceled = 2,
	}
	
	/// <summary>
	/// Activity that is created automatically when an opportunity is closed, containing information such as the description of the closing and actual revenue.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("opportunityclose")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class OpportunityClose : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string ActivityId = "activityid";
			public const string Id = "activityid";
			public const string ActivityTypeCode = "activitytypecode";
			public const string ActualDurationMinutes = "actualdurationminutes";
			public const string ActualEnd = "actualend";
			public const string ActualRevenue = "actualrevenue";
			public const string ActualRevenue_Base = "actualrevenue_base";
			public const string ActualStart = "actualstart";
			public const string Category = "category";
			public const string CompetitorId = "competitorid";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string Description = "description";
			public const string ExchangeRate = "exchangerate";
			public const string ImportSequenceNumber = "importsequencenumber";
			public const string IsBilled = "isbilled";
			public const string IsRegularActivity = "isregularactivity";
			public const string IsWorkflowCreated = "isworkflowcreated";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string OpportunityId = "opportunityid";
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
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string TransactionCurrencyId = "transactioncurrencyid";
			public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
			public const string VersionNumber = "versionnumber";
			public const string activity_pointer_opportunity_close = "activity_pointer_opportunity_close";
			public const string business_unit_opportunity_close_activities = "business_unit_opportunity_close_activities";
			public const string competitor_opportunity_activities = "competitor_opportunity_activities";
			public const string lk_opportunityclose_createdby = "lk_opportunityclose_createdby";
			public const string lk_opportunityclose_createdonbehalfby = "lk_opportunityclose_createdonbehalfby";
			public const string lk_opportunityclose_modifiedby = "lk_opportunityclose_modifiedby";
			public const string lk_opportunityclose_modifiedonbehalfby = "lk_opportunityclose_modifiedonbehalfby";
			public const string Opportunity_OpportunityClose = "Opportunity_OpportunityClose";
			public const string service_opportunityclose = "service_opportunityclose";
			public const string team_opportunityclose = "team_opportunityclose";
			public const string transactioncurrency_opportunityclose = "transactioncurrency_opportunityclose";
			public const string user_opportunityclose = "user_opportunityclose";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public OpportunityClose() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "opportunityclose";
		
		public const string EntitySchemaName = "OpportunityClose";
		
		public const string PrimaryIdAttribute = "activityid";
		
		public const string PrimaryNameAttribute = "subject";
		
		public const int EntityTypeCode = 4208;
		
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
		/// Unique identifier of the opportunity close activity.
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
		/// Actual duration of the opportunity close activity in minutes.
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
		/// Actual end time of the opportunity close activity.
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
		/// Actual revenue generated for the opportunity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualrevenue")]
		public Microsoft.Xrm.Sdk.Money ActualRevenue
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("actualrevenue");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ActualRevenue");
				this.SetAttributeValue("actualrevenue", value);
				this.OnPropertyChanged("ActualRevenue");
			}
		}
		
		/// <summary>
		/// Shows the Actual Revenue field converted to the system's default base currency for reporting purposes. The calculation uses the exchange rate specified in the Currencies area.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualrevenue_base")]
		public Microsoft.Xrm.Sdk.Money ActualRevenue_Base
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("actualrevenue_base");
			}
		}
		
		/// <summary>
		/// Actual start time of the opportunity close activity.
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
		/// Category of the opportunity close activity.
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
		/// Unique identifier of the competitor with which the opportunity close activity is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("competitorid")]
		public Microsoft.Xrm.Sdk.EntityReference CompetitorId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("competitorid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CompetitorId");
				this.SetAttributeValue("competitorid", value);
				this.OnPropertyChanged("CompetitorId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the opportunity close activity.
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
		/// Date and time when the opportunity close activity was created.
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
		/// Unique identifier of the delegate user who created the opportunityclose.
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
		/// Activity that is created automatically when an opportunity is closed, containing information such as the description of the closing and actual revenue.
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
		/// Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("exchangerate")]
		public System.Nullable<decimal> ExchangeRate
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("exchangerate");
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
		/// Information about whether the opportunity close activity was billed as part of resolving a case.
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
		/// Information that specifies if the opportunity close activity was created from a workflow rule.
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
		/// Unique identifier of the user who last modified the opportunity close activity.
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
		/// Date and time when the opportunity close activity was last modified.
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
		/// Unique identifier of the delegate user who last modified the opportunityclose.
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
		/// Unique identifier of the opportunity closed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("opportunityid")]
		public Microsoft.Xrm.Sdk.EntityReference OpportunityId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("opportunityid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OpportunityId");
				this.SetAttributeValue("opportunityid", value);
				this.OnPropertyChanged("OpportunityId");
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
		/// Unique identifier of the user or team who owns the opportunity close activity.
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
		/// Unique identifier of the business unit that owns the opportunity close activity.
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
		/// Unique identifier of the team who owns the opportunity close activity.
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
		/// Unique identifier of the user who owns the opportunity close activity.
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
		/// Scheduled duration of the opportunity close activity, specified in minutes.
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
		/// Scheduled end time of the opportunity close activity.
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
		/// Scheduled start time of the opportunity close activity.
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
		/// Unique identifier of the service with which the opportunity close activity is associated.
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
		/// Shows whether the opportunity close activity is open, completed, or canceled. By default, opportunity close activities are completed unless the opportunity is reactivated, which updates them to canceled.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<Ocas.Domestic.Crm.Entities.OpportunityCloseState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((Ocas.Domestic.Crm.Entities.OpportunityCloseState)(System.Enum.ToObject(typeof(Ocas.Domestic.Crm.Entities.OpportunityCloseState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Reason for the status of the opportunity close activity.
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
		/// Subcategory of the opportunity close activity.
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
		/// Subject associated with the opportunity close activity.
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
		/// Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		public Microsoft.Xrm.Sdk.EntityReference TransactionCurrencyId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("transactioncurrencyid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TransactionCurrencyId");
				this.SetAttributeValue("transactioncurrencyid", value);
				this.OnPropertyChanged("TransactionCurrencyId");
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
		/// Version number of the opportunity close activity.
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
		/// 1:N opportunityclose_activity_parties
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("opportunityclose_activity_parties")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.ActivityParty> opportunityclose_activity_parties
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityParty>("opportunityclose_activity_parties", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("opportunityclose_activity_parties");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.ActivityParty>("opportunityclose_activity_parties", null, value);
				this.OnPropertyChanged("opportunityclose_activity_parties");
			}
		}
		
		/// <summary>
		/// 1:N OpportunityClose_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("OpportunityClose_Annotation")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> OpportunityClose_Annotation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("OpportunityClose_Annotation", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OpportunityClose_Annotation");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("OpportunityClose_Annotation", null, value);
				this.OnPropertyChanged("OpportunityClose_Annotation");
			}
		}
		
		/// <summary>
		/// 1:N OpportunityClose_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("OpportunityClose_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> OpportunityClose_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("OpportunityClose_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OpportunityClose_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("OpportunityClose_AsyncOperations", null, value);
				this.OnPropertyChanged("OpportunityClose_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N OpportunityClose_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("OpportunityClose_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> OpportunityClose_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("OpportunityClose_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("OpportunityClose_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("OpportunityClose_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("OpportunityClose_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N userentityinstancedata_opportunityclose
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_opportunityclose")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_opportunityclose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_opportunityclose", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_opportunityclose");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_opportunityclose", null, value);
				this.OnPropertyChanged("userentityinstancedata_opportunityclose");
			}
		}
		
		/// <summary>
		/// N:1 activity_pointer_opportunity_close
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("activity_pointer_opportunity_close")]
		public Ocas.Domestic.Crm.Entities.ActivityPointer activity_pointer_opportunity_close
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.ActivityPointer>("activity_pointer_opportunity_close", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("activity_pointer_opportunity_close");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.ActivityPointer>("activity_pointer_opportunity_close", null, value);
				this.OnPropertyChanged("activity_pointer_opportunity_close");
			}
		}
		
		/// <summary>
		/// N:1 business_unit_opportunity_close_activities
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("business_unit_opportunity_close_activities")]
		public Ocas.Domestic.Crm.Entities.BusinessUnit business_unit_opportunity_close_activities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.BusinessUnit>("business_unit_opportunity_close_activities", null);
			}
		}
		
		/// <summary>
		/// N:1 competitor_opportunity_activities
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("competitorid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("competitor_opportunity_activities")]
		public Ocas.Domestic.Crm.Entities.Competitor competitor_opportunity_activities
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Competitor>("competitor_opportunity_activities", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("competitor_opportunity_activities");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Competitor>("competitor_opportunity_activities", null, value);
				this.OnPropertyChanged("competitor_opportunity_activities");
			}
		}
		
		/// <summary>
		/// N:1 lk_opportunityclose_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_opportunityclose_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_opportunityclose_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_opportunityclose_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_opportunityclose_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_opportunityclose_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_opportunityclose_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_opportunityclose_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_opportunityclose_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_opportunityclose_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_opportunityclose_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_opportunityclose_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_opportunityclose_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_opportunityclose_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_opportunityclose_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_opportunityclose_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_opportunityclose_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 Opportunity_OpportunityClose
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("opportunityid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Opportunity_OpportunityClose")]
		public Ocas.Domestic.Crm.Entities.Opportunity Opportunity_OpportunityClose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Opportunity>("Opportunity_OpportunityClose", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Opportunity_OpportunityClose");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Opportunity>("Opportunity_OpportunityClose", null, value);
				this.OnPropertyChanged("Opportunity_OpportunityClose");
			}
		}
		
		/// <summary>
		/// N:1 service_opportunityclose
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("serviceid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("service_opportunityclose")]
		public Ocas.Domestic.Crm.Entities.Service service_opportunityclose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Service>("service_opportunityclose", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("service_opportunityclose");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Service>("service_opportunityclose", null, value);
				this.OnPropertyChanged("service_opportunityclose");
			}
		}
		
		/// <summary>
		/// N:1 team_opportunityclose
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_opportunityclose")]
		public Ocas.Domestic.Crm.Entities.Team team_opportunityclose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Team>("team_opportunityclose", null);
			}
		}
		
		/// <summary>
		/// N:1 transactioncurrency_opportunityclose
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("transactioncurrency_opportunityclose")]
		public Ocas.Domestic.Crm.Entities.TransactionCurrency transactioncurrency_opportunityclose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.TransactionCurrency>("transactioncurrency_opportunityclose", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("transactioncurrency_opportunityclose");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.TransactionCurrency>("transactioncurrency_opportunityclose", null, value);
				this.OnPropertyChanged("transactioncurrency_opportunityclose");
			}
		}
		
		/// <summary>
		/// N:1 user_opportunityclose
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("user_opportunityclose")]
		public Ocas.Domestic.Crm.Entities.SystemUser user_opportunityclose
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("user_opportunityclose", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual OpportunityClose_StatusCode? StatusCodeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((OpportunityClose_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				StatusCode = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}