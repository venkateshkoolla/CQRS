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
	/// 
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("roletemplateprivileges")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class RoleTemplatePrivileges : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string IsBasic = "isbasic";
			public const string IsDeep = "isdeep";
			public const string IsGlobal = "isglobal";
			public const string IsLocal = "islocal";
			public const string PrivilegeId = "privilegeid";
			public const string RoleTemplateId = "roletemplateid";
			public const string RoleTemplatePrivilegeId = "roletemplateprivilegeid";
			public const string Id = "roletemplateprivilegeid";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public RoleTemplatePrivileges() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "roletemplateprivileges";
		
		public const string EntitySchemaName = "RoleTemplatePrivileges";
		
		public const string PrimaryIdAttribute = "roletemplateprivilegeid";
		
		public const int EntityTypeCode = 28;
		
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
		/// Information about whether the role in the template applies to the user, the user's team, or objects shared by the user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isbasic")]
		public System.Nullable<bool> IsBasic
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isbasic");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsBasic");
				this.SetAttributeValue("isbasic", value);
				this.OnPropertyChanged("IsBasic");
			}
		}
		
		/// <summary>
		/// Information about whether the role in the template applies to child business units of the business unit associated with the user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isdeep")]
		public System.Nullable<bool> IsDeep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isdeep");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsDeep");
				this.SetAttributeValue("isdeep", value);
				this.OnPropertyChanged("IsDeep");
			}
		}
		
		/// <summary>
		/// Information about whether the role in the template applies to the entire organization.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isglobal")]
		public System.Nullable<bool> IsGlobal
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isglobal");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsGlobal");
				this.SetAttributeValue("isglobal", value);
				this.OnPropertyChanged("IsGlobal");
			}
		}
		
		/// <summary>
		/// Information about whether the role in the template applies to the user's business unit.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("islocal")]
		public System.Nullable<bool> IsLocal
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("islocal");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsLocal");
				this.SetAttributeValue("islocal", value);
				this.OnPropertyChanged("IsLocal");
			}
		}
		
		/// <summary>
		/// Unique identifier of the privilege assigned to the role template.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("privilegeid")]
		public System.Nullable<System.Guid> PrivilegeId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("privilegeid");
			}
		}
		
		/// <summary>
		/// Unique identifier of the role template that is associated with the role privilege.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("roletemplateid")]
		public System.Nullable<System.Guid> RoleTemplateId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("roletemplateid");
			}
		}
		
		/// <summary>
		/// Unique identifier of the role template privileges.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("roletemplateprivilegeid")]
		public System.Nullable<System.Guid> RoleTemplatePrivilegeId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("roletemplateprivilegeid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("RoleTemplatePrivilegeId");
				this.SetAttributeValue("roletemplateprivilegeid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("RoleTemplatePrivilegeId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("roletemplateprivilegeid")]
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
				this.RoleTemplatePrivilegeId = value;
			}
		}
	}
}