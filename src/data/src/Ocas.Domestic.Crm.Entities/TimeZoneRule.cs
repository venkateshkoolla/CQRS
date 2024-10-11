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
	/// Definition for time conversion between local time and Coordinated Universal Time (UTC) for a particular time zone at a particular time period.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("timezonerule")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class TimeZoneRule : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string Bias = "bias";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string DaylightBias = "daylightbias";
			public const string DaylightDay = "daylightday";
			public const string DaylightDayOfWeek = "daylightdayofweek";
			public const string DaylightHour = "daylighthour";
			public const string DaylightMinute = "daylightminute";
			public const string DaylightMonth = "daylightmonth";
			public const string DaylightSecond = "daylightsecond";
			public const string DaylightYear = "daylightyear";
			public const string EffectiveDateTime = "effectivedatetime";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string OrganizationId = "organizationid";
			public const string StandardBias = "standardbias";
			public const string StandardDay = "standardday";
			public const string StandardDayOfWeek = "standarddayofweek";
			public const string StandardHour = "standardhour";
			public const string StandardMinute = "standardminute";
			public const string StandardMonth = "standardmonth";
			public const string StandardSecond = "standardsecond";
			public const string StandardYear = "standardyear";
			public const string TimeZoneDefinitionId = "timezonedefinitionid";
			public const string TimeZoneRuleId = "timezoneruleid";
			public const string Id = "timezoneruleid";
			public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
			public const string VersionNumber = "versionnumber";
			public const string lk_timezonerule_createdby = "lk_timezonerule_createdby";
			public const string lk_timezonerule_createdonbehalfby = "lk_timezonerule_createdonbehalfby";
			public const string lk_timezonerule_modifiedby = "lk_timezonerule_modifiedby";
			public const string lk_timezonerule_modifiedonbehalfby = "lk_timezonerule_modifiedonbehalfby";
			public const string lk_timezonerule_timezonedefinitionid = "lk_timezonerule_timezonedefinitionid";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public TimeZoneRule() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "timezonerule";
		
		public const string EntitySchemaName = "TimeZoneRule";
		
		public const string PrimaryIdAttribute = "timezoneruleid";
		
		public const string PrimaryNameAttribute = "timezoneruleversionnumber";
		
		public const int EntityTypeCode = 4811;
		
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
		/// Base time bias of the time zone rule.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bias")]
		public System.Nullable<int> Bias
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("bias");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Bias");
				this.SetAttributeValue("bias", value);
				this.OnPropertyChanged("Bias");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the time zone rule.
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
		/// Date and time when the time zone rule was created.
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
		/// Unique identifier of the delegate user who created the timezonerule.
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
		/// Time bias in addition to the base bias for daylight savings time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightbias")]
		public System.Nullable<int> DaylightBias
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightbias");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightBias");
				this.SetAttributeValue("daylightbias", value);
				this.OnPropertyChanged("DaylightBias");
			}
		}
		
		/// <summary>
		/// Day of the month when daylight savings time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightday")]
		public System.Nullable<int> DaylightDay
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightday");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightDay");
				this.SetAttributeValue("daylightday", value);
				this.OnPropertyChanged("DaylightDay");
			}
		}
		
		/// <summary>
		/// Day of the week when daylight savings time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightdayofweek")]
		public System.Nullable<int> DaylightDayOfWeek
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightdayofweek");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightDayOfWeek");
				this.SetAttributeValue("daylightdayofweek", value);
				this.OnPropertyChanged("DaylightDayOfWeek");
			}
		}
		
		/// <summary>
		/// Hour of the day when daylight savings time starts
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylighthour")]
		public System.Nullable<int> DaylightHour
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylighthour");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightHour");
				this.SetAttributeValue("daylighthour", value);
				this.OnPropertyChanged("DaylightHour");
			}
		}
		
		/// <summary>
		/// Minute of the hour when daylight savings time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightminute")]
		public System.Nullable<int> DaylightMinute
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightminute");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightMinute");
				this.SetAttributeValue("daylightminute", value);
				this.OnPropertyChanged("DaylightMinute");
			}
		}
		
		/// <summary>
		/// Month when daylight savings time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightmonth")]
		public System.Nullable<int> DaylightMonth
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightmonth");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightMonth");
				this.SetAttributeValue("daylightmonth", value);
				this.OnPropertyChanged("DaylightMonth");
			}
		}
		
		/// <summary>
		/// Second of the minute when daylight savings time starts
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightsecond")]
		public System.Nullable<int> DaylightSecond
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightsecond");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightSecond");
				this.SetAttributeValue("daylightsecond", value);
				this.OnPropertyChanged("DaylightSecond");
			}
		}
		
		/// <summary>
		/// Year when daylight savings times starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("daylightyear")]
		public System.Nullable<int> DaylightYear
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("daylightyear");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("DaylightYear");
				this.SetAttributeValue("daylightyear", value);
				this.OnPropertyChanged("DaylightYear");
			}
		}
		
		/// <summary>
		/// Time that this rule takes effect, in local time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("effectivedatetime")]
		public System.Nullable<System.DateTime> EffectiveDateTime
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("effectivedatetime");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("EffectiveDateTime");
				this.SetAttributeValue("effectivedatetime", value);
				this.OnPropertyChanged("EffectiveDateTime");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the time zone rule.
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
		/// Date and time when the time zone rule was modified.
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
		/// Unique identifier of the delegate user who last modified the timezonerule.
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
		/// Unique identifier of the organization associated with the time zone rule.
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
		/// Time bias in addition to the base bias for standard time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardbias")]
		public System.Nullable<int> StandardBias
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardbias");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardBias");
				this.SetAttributeValue("standardbias", value);
				this.OnPropertyChanged("StandardBias");
			}
		}
		
		/// <summary>
		/// Day of the month when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardday")]
		public System.Nullable<int> StandardDay
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardday");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardDay");
				this.SetAttributeValue("standardday", value);
				this.OnPropertyChanged("StandardDay");
			}
		}
		
		/// <summary>
		/// Day of the week when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standarddayofweek")]
		public System.Nullable<int> StandardDayOfWeek
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standarddayofweek");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardDayOfWeek");
				this.SetAttributeValue("standarddayofweek", value);
				this.OnPropertyChanged("StandardDayOfWeek");
			}
		}
		
		/// <summary>
		/// Hour of the day when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardhour")]
		public System.Nullable<int> StandardHour
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardhour");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardHour");
				this.SetAttributeValue("standardhour", value);
				this.OnPropertyChanged("StandardHour");
			}
		}
		
		/// <summary>
		/// Minute of the hour when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardminute")]
		public System.Nullable<int> StandardMinute
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardminute");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardMinute");
				this.SetAttributeValue("standardminute", value);
				this.OnPropertyChanged("StandardMinute");
			}
		}
		
		/// <summary>
		/// Month when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardmonth")]
		public System.Nullable<int> StandardMonth
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardmonth");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardMonth");
				this.SetAttributeValue("standardmonth", value);
				this.OnPropertyChanged("StandardMonth");
			}
		}
		
		/// <summary>
		/// Second of the Minute when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardsecond")]
		public System.Nullable<int> StandardSecond
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardsecond");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardSecond");
				this.SetAttributeValue("standardsecond", value);
				this.OnPropertyChanged("StandardSecond");
			}
		}
		
		/// <summary>
		/// Year when standard time starts.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("standardyear")]
		public System.Nullable<int> StandardYear
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("standardyear");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StandardYear");
				this.SetAttributeValue("standardyear", value);
				this.OnPropertyChanged("StandardYear");
			}
		}
		
		/// <summary>
		/// Unique identifier of the time zone definition.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezonedefinitionid")]
		public Microsoft.Xrm.Sdk.EntityReference TimeZoneDefinitionId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("timezonedefinitionid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TimeZoneDefinitionId");
				this.SetAttributeValue("timezonedefinitionid", value);
				this.OnPropertyChanged("TimeZoneDefinitionId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the time zone rule.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleid")]
		public System.Nullable<System.Guid> TimeZoneRuleId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("timezoneruleid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("TimeZoneRuleId");
				this.SetAttributeValue("timezoneruleid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("TimeZoneRuleId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleid")]
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
				this.TimeZoneRuleId = value;
			}
		}
		
		/// <summary>
		/// For internal use only
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
		/// 1:N userentityinstancedata_timezonerule
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_timezonerule")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_timezonerule
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_timezonerule", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_timezonerule");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_timezonerule", null, value);
				this.OnPropertyChanged("userentityinstancedata_timezonerule");
			}
		}
		
		/// <summary>
		/// N:1 lk_timezonerule_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_timezonerule_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_timezonerule_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_timezonerule_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_timezonerule_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_timezonerule_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_timezonerule_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_timezonerule_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_timezonerule_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_timezonerule_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_timezonerule_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_timezonerule_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_timezonerule_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_timezonerule_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_timezonerule_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_timezonerule_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_timezonerule_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_timezonerule_timezonedefinitionid
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezonedefinitionid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_timezonerule_timezonedefinitionid")]
		public Ocas.Domestic.Crm.Entities.TimeZoneDefinition lk_timezonerule_timezonedefinitionid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.TimeZoneDefinition>("lk_timezonerule_timezonedefinitionid", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_timezonerule_timezonedefinitionid");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.TimeZoneDefinition>("lk_timezonerule_timezonedefinitionid", null, value);
				this.OnPropertyChanged("lk_timezonerule_timezonedefinitionid");
			}
		}
	}
}