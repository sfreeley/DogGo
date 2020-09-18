using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int NeighborhoodId { get; set; }
        //this will display the name of the neighborhood instead of having to display the neighborhoodId
        public Neighborhood Neighborhood { get; set; }
        public string Phone { get; set; }

    }
}
