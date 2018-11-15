using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Models
{
    public abstract class DateTrackingModel : ModelBase
    {
        public DateTime DateCreated { get; internal set; }
        public DateTime DateModified { get; internal set; }
    }
}
