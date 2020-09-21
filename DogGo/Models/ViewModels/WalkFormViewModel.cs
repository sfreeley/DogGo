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
       
        //asp.net class that represents the selected item objects (this will be the dogs that are selected from the listbox rendered from the cshtml)
       
       public IEnumerable<SelectListItem> DogsList { get; set; }
        //this will be a collection of dogs that are selected from the dog list and will be the id of the dog as a string 
       public IEnumerable<string> SelectedDogs { get; set; }
       
    }
}
