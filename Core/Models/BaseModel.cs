using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class BaseModel
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        protected BaseModel() { }

        public BaseModel(DateTime creationDate, DateTime? modifiedDate = null) : this()
        {
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
        }
    }
}
