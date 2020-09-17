using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class DogOwnerViewModel
    {
        public Owner Owner { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}
