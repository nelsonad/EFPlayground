using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Models
{
    public enum CustomerSource
    {
        [Display(Name="Direct Entry")]
        DirectEntry,
        Email
    }
}
