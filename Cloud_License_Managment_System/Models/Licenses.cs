﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Cloud_License_Managment_System.Models
{
    public partial class Licenses
    {
        public long Id { get; set; }
        public string LisenceNumber { get; set; }
        public long ProductId { get; set; }
        public string Status { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExperiDate { get; set; }
    }
}