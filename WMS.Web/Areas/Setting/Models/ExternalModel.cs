using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMS.Web.Areas.Setting.Models
{
    public class ExternalModel
    {
        [Required]
        [DisplayName("System Code")]
        public string SystemCode { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("URL")]
        public string URL { get; set; }

    }
}