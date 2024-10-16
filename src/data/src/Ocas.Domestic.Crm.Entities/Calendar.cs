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
	/// Calendar used by the scheduling system to define when an appointment or activity is to occur.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("calendar")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class Calendar : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string BusinessUnitId = "businessunitid";
			public const string CalendarId = "calendarid";
			public const string Id = "calendarid";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string Description = "description";
			public const string HolidayScheduleCalendarId = "holidayschedulecalendarid";
			public const string IsShared = "isshared";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string Name = "name";
			public const string OrganizationId = "organizationid";
			public const string PrimaryUserId = "primaryuserid";
			public const string Type = "type";
			public const string VersionNumber = "versionnumber";
			public const string CalendarRules = "calendarrules";
			public const string business_unit_calendars = "business_unit_calendars";
			public const string Referencingcalendar_customercalendar_holidaycalendar = "calendar_customercalendar_holidaycalendar";
			public const string lk_calendar_createdby = "lk_calendar_createdby";
			public const string lk_calendar_createdonbehalfby = "lk_calendar_createdonbehalfby";
			public const string lk_calendar_modifiedby = "lk_calendar_modifiedby";
			public const string lk_calendar_modifiedonbehalfby = "lk_calendar_modifiedonbehalfby";
			public const string organization_calendars = "organization_calendars";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public Calendar() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "calendar";
		
		public const string EntitySchemaName = "Calendar";
		
		public const string PrimaryIdAttribute = "calendarid";
		
		public const string PrimaryNameAttribute = "name";
		
		public const int EntityTypeCode = 4003;
		
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
		/// Unique identifier of the business unit with which the calendar is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("businessunitid")]
		public Microsoft.Xrm.Sdk.EntityReference BusinessUnitId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("businessunitid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("BusinessUnitId");
				this.SetAttributeValue("businessunitid", value);
				this.OnPropertyChanged("BusinessUnitId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the calendar.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("calendarid")]
		public System.Nullable<System.Guid> CalendarId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("calendarid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CalendarId");
				this.SetAttributeValue("calendarid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("CalendarId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("calendarid")]
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
				this.CalendarId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the calendar.
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
		/// Date and time when the calendar was created.
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
		/// Unique identifier of the delegate user who created the calendar.
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
		/// Calendar used by the scheduling system to define when an appointment or activity is to occur.
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
		/// Holiday Schedule CalendarId
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("holidayschedulecalendarid")]
		public Microsoft.Xrm.Sdk.EntityReference HolidayScheduleCalendarId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("holidayschedulecalendarid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("HolidayScheduleCalendarId");
				this.SetAttributeValue("holidayschedulecalendarid", value);
				this.OnPropertyChanged("HolidayScheduleCalendarId");
			}
		}
		
		/// <summary>
		/// Calendar is shared by other calendars, such as the organization calendar.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isshared")]
		public System.Nullable<bool> IsShared
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isshared");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsShared");
				this.SetAttributeValue("isshared", value);
				this.OnPropertyChanged("IsShared");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the calendar.
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
		/// Date and time when the calendar was last modified.
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
		/// Unique identifier of the delegate user who last modified the calendar.
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
		/// Name of the calendar.
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
		/// Unique identifier of the organization with which the calendar is associated.
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
		/// Unique identifier of the primary user of this calendar.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("primaryuserid")]
		public System.Nullable<System.Guid> PrimaryUserId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("primaryuserid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("PrimaryUserId");
				this.SetAttributeValue("primaryuserid", value);
				this.OnPropertyChanged("PrimaryUserId");
			}
		}
		
		/// <summary>
		/// Calendar type, such as User work hour calendar, or Customer service hour calendar.
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
		/// 1:N BusinessUnit_Calendar
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("BusinessUnit_Calendar")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BusinessUnit> BusinessUnit_Calendar
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BusinessUnit>("BusinessUnit_Calendar", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("BusinessUnit_Calendar");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BusinessUnit>("BusinessUnit_Calendar", null, value);
				this.OnPropertyChanged("BusinessUnit_Calendar");
			}
		}
		
		/// <summary>
		/// 1:N Calendar_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Calendar_Annotation")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Annotation> Calendar_Annotation
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("Calendar_Annotation", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Calendar_Annotation");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Annotation>("Calendar_Annotation", null, value);
				this.OnPropertyChanged("Calendar_Annotation");
			}
		}
		
		/// <summary>
		/// 1:N Calendar_AsyncOperations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Calendar_AsyncOperations")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.AsyncOperation> Calendar_AsyncOperations
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("Calendar_AsyncOperations", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Calendar_AsyncOperations");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.AsyncOperation>("Calendar_AsyncOperations", null, value);
				this.OnPropertyChanged("Calendar_AsyncOperations");
			}
		}
		
		/// <summary>
		/// 1:N Calendar_BulkDeleteFailures
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Calendar_BulkDeleteFailures")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.BulkDeleteFailure> Calendar_BulkDeleteFailures
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("Calendar_BulkDeleteFailures", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Calendar_BulkDeleteFailures");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.BulkDeleteFailure>("Calendar_BulkDeleteFailures", null, value);
				this.OnPropertyChanged("Calendar_BulkDeleteFailures");
			}
		}
		
		/// <summary>
		/// 1:N calendar_calendar_rules
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("calendarrules")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.CalendarRule> CalendarRules
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.EntityCollection collection = this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityCollection>("calendarrules");
				if (((collection != null) 
							&& (collection.Entities != null)))
				{
					return System.Linq.Enumerable.Cast<Ocas.Domestic.Crm.Entities.CalendarRule>(collection.Entities);
				}
				else
				{
					return null;
				}
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CalendarRules");
				if ((value == null))
				{
					this.SetAttributeValue("calendarrules", value);
				}
				else
				{
					this.SetAttributeValue("calendarrules", new Microsoft.Xrm.Sdk.EntityCollection(new System.Collections.Generic.List<Microsoft.Xrm.Sdk.Entity>(value)));
				}
				this.OnPropertyChanged("CalendarRules");
			}
		}
		
		/// <summary>
		/// 1:N calendar_customercalendar_holidaycalendar
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Calendar> Referencedcalendar_customercalendar_holidaycalendar
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Calendar>("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Referencedcalendar_customercalendar_holidaycalendar");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Calendar>("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
				this.OnPropertyChanged("Referencedcalendar_customercalendar_holidaycalendar");
			}
		}
		
		/// <summary>
		/// 1:N calendar_equipment
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_equipment")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Equipment> calendar_equipment
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Equipment>("calendar_equipment", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("calendar_equipment");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Equipment>("calendar_equipment", null, value);
				this.OnPropertyChanged("calendar_equipment");
			}
		}
		
		/// <summary>
		/// 1:N calendar_organization
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_organization")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Organization> calendar_organization
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Organization>("calendar_organization", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("calendar_organization");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Organization>("calendar_organization", null, value);
				this.OnPropertyChanged("calendar_organization");
			}
		}
		
		/// <summary>
		/// 1:N calendar_services
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_services")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.Service> calendar_services
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.Service>("calendar_services", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("calendar_services");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.Service>("calendar_services", null, value);
				this.OnPropertyChanged("calendar_services");
			}
		}
		
		/// <summary>
		/// 1:N calendar_system_users
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_system_users")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SystemUser> calendar_system_users
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SystemUser>("calendar_system_users", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("calendar_system_users");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SystemUser>("calendar_system_users", null, value);
				this.OnPropertyChanged("calendar_system_users");
			}
		}
		
		/// <summary>
		/// 1:N slabase_businesshoursid
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("slabase_businesshoursid")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.SLA> slabase_businesshoursid
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.SLA>("slabase_businesshoursid", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("slabase_businesshoursid");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.SLA>("slabase_businesshoursid", null, value);
				this.OnPropertyChanged("slabase_businesshoursid");
			}
		}
		
		/// <summary>
		/// 1:N userentityinstancedata_calendar
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("userentityinstancedata_calendar")]
		public System.Collections.Generic.IEnumerable<Ocas.Domestic.Crm.Entities.UserEntityInstanceData> userentityinstancedata_calendar
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_calendar", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("userentityinstancedata_calendar");
				this.SetRelatedEntities<Ocas.Domestic.Crm.Entities.UserEntityInstanceData>("userentityinstancedata_calendar", null, value);
				this.OnPropertyChanged("userentityinstancedata_calendar");
			}
		}
		
		/// <summary>
		/// N:1 business_unit_calendars
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("businessunitid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("business_unit_calendars")]
		public Ocas.Domestic.Crm.Entities.BusinessUnit business_unit_calendars
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.BusinessUnit>("business_unit_calendars", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("business_unit_calendars");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.BusinessUnit>("business_unit_calendars", null, value);
				this.OnPropertyChanged("business_unit_calendars");
			}
		}
		
		/// <summary>
		/// N:1 calendar_customercalendar_holidaycalendar
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("holidayschedulecalendarid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public Ocas.Domestic.Crm.Entities.Calendar Referencingcalendar_customercalendar_holidaycalendar
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Calendar>("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Referencingcalendar_customercalendar_holidaycalendar");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.Calendar>("calendar_customercalendar_holidaycalendar", Microsoft.Xrm.Sdk.EntityRole.Referencing, value);
				this.OnPropertyChanged("Referencingcalendar_customercalendar_holidaycalendar");
			}
		}
		
		/// <summary>
		/// N:1 lk_calendar_createdby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_calendar_createdby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_calendar_createdby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_createdby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_calendar_createdonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_calendar_createdonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_calendar_createdonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_createdonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_calendar_createdonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_createdonbehalfby", null, value);
				this.OnPropertyChanged("lk_calendar_createdonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 lk_calendar_modifiedby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_calendar_modifiedby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_calendar_modifiedby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_modifiedby", null);
			}
		}
		
		/// <summary>
		/// N:1 lk_calendar_modifiedonbehalfby
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_calendar_modifiedonbehalfby")]
		public Ocas.Domestic.Crm.Entities.SystemUser lk_calendar_modifiedonbehalfby
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_modifiedonbehalfby", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("lk_calendar_modifiedonbehalfby");
				this.SetRelatedEntity<Ocas.Domestic.Crm.Entities.SystemUser>("lk_calendar_modifiedonbehalfby", null, value);
				this.OnPropertyChanged("lk_calendar_modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// N:1 organization_calendars
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_calendars")]
		public Ocas.Domestic.Crm.Entities.Organization organization_calendars
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<Ocas.Domestic.Crm.Entities.Organization>("organization_calendars", null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("type")]
		public virtual Calendar_Type? TypeEnum
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((Calendar_Type?)(EntityOptionSetEnum.GetEnum(this, "type")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				Type = value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null;
			}
		}
	}
}