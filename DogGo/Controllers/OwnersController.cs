using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public OwnersController(IOwnerRepository ownerRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository, INeighborhoodRepository neighborhoodRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        //this will show the login form to the user
        public ActionResult Login()
        {
            return View();
        }

        //specify this is the Login POST method
        //when user clicks submit to login with email, this code will run
        //take in an instance of the LoginViewModel
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            //getting one owner based on their email address -- we are seeing if the owner's typed in email address matches a user's email address in the database
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            //if there is no owner found with that typed in email address, return that they are not authorized
            if (owner == null)
            {
                return Unauthorized();            
            }

            //otherwise, keep going (claims will be a list of properties we want to remember and store about the user 
            var claims = new List<Claim>
            {
                //this is the owner's Id, which is generally an identifier that is most commonly used across the board in authentication and authorization;
                //all values will be stored as strings
                new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
                new Claim(ClaimTypes.Email, owner.Email),
                new Claim(ClaimTypes.Role, "DogOwner"),
            };

            //saving user/owner data into a cookie (ie the claims list with the information we chose to store: email, id, role) --this allows us to store data on a user's browser
            //the server will then take that cookie and return it to whoever made that request;
            //in the future, the server will know when that user is making a request and will send the value of the data that's stored in that cookie 
            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //this code will sign in the user;
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

            //will bring user/owner to Dog's Index page
            return RedirectToAction("Index", "Dogs");
        }
        // GET: OwnersController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnersController/Details/5
        //this owners controller will get the id number from the url and that is what is
        //being passed into the Details method;
        public ActionResult Details(int id)
        {
            //get the one owner by Id (user is clicking on this owner)
            Owner owner = _ownerRepo.GetOwnerById(id);
       
            //pass this owner's Id into the method that gets a list of dogs by that ownerId
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);

            //get a list of walkers by the owner's neighborhoodId  
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            //create new instance of your ProfileViewModel
            ProfileViewModel vm = new ProfileViewModel()
            {
                //these are setting the properties in your ProfileViewModel class to their respective values
                //which we obtained from above;
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            //controller class then passes this information to the Razor view;
            return View(vm);
        }

        // GET: OwnersController/Create
        //this method only creates the form and displays it to the user
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //this method will actually submit or POST the form
        //and then bring the user back to the index page;
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        // controller will get an owner id from the url route
        // use that id to retrieve that specific data from the database
        // use it to prepopulate the form with the information the user will be editing
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = owner,
                Neighborhoods = neighborhoods
            };

            return View(vm);
            //if (owner == null)
            //{
            //    return NotFound();
            //}

            //return View(owner);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);
                return RedirectToAction("Index");
            }
            catch
            {
                OwnerFormViewModel vm = new OwnerFormViewModel()
                {
                    Owner = owner,
                    Neighborhoods = _neighborhoodRepo.GetAll()
                };
            return View(vm);
        }
            
    }

        // GET: OwnersController/Delete/5
        //this will display the delete message and ask if you are sure
        //if you want to delete this owner;
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            return View(owner);
        }

        // POST: OwnersController/Delete/5
        //this is the actual method that will delete the owner from the database;
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
