//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LeaveApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Leave
    {
        public string ID { get; set; }
        public string LeaveDescription { get; set; }
        public string TempContact { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public short LeaveTypeCount { get; set; }
        public short TotalLeaveCount { get; set; }
    
        public virtual Teacher Teacher { get; set; }
    }
}
