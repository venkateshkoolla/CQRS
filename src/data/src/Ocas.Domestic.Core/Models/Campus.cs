﻿using System;

namespace Ocas.Domestic.Models
{
    public class Campus : IAccount
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string LocalizedName { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public Address MailingAddress { get; set; }
    }
}
