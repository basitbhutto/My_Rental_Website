//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OlxWebsiteFyp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Regstration
    {
        public int re_id { get; set; }

        [Display(Name = "Name")]
        public string re_name { get; set; }

        [Display(Name = "Gmail")]
        public string re_email { get; set; }

        [Display(Name = "Gender")]
        public string re_gender { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string re_age { get; set; }

        [Display(Name = "Image")]
        public string re_Photo { get; set; }

        [Display(Name = "City")]
        public string re_city { get; set; }

        [Display(Name = "Contact")]
        public string re_contact { get; set; }

        [Display(Name = "Type")]
        public Nullable<int> t_id { get; set; }
    
        public virtual Type Type { get; set; }
    }
}
