using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
       public Walk Walk { get; set; }
       public List<Walker> Walkers { get; set; } 
       public List<Dog> Dogs { get; set; }
       public IEnumerable<SelectListItem> DogsList { get; set; }
       public IEnumerable<string> SelectedDogs { get; set; }
       
    }
}
