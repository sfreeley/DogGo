using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{ //creating a ViewModel that will allow us to connect a dog with it's owner
    public class DogFormViewModel
    {
        public Dog Dog { get; set; }
        public List<Owner> Owners { get; set;}
    }
}
