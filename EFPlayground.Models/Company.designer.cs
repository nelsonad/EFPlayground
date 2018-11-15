using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Models
{
    public abstract class CompanyBase : DateTrackingModel
    {
        [Required]
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
