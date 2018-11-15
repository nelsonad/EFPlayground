using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Models
{
    public class Customer : CustomerBase
    {
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        //public string DisplayName { get; set; }
    }
}
