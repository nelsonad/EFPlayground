using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Models
{
    public abstract class CustomerBase : DateTrackingModel
    {
        public CustomerBase()
        {
            Groups = new HashSet<Group>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public CustomerSource Source { get; set; }
        
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
