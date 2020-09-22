using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    //there are routes we don't want a user that is not authenticated to view and make changes; this attribute will not allow a user that is not logged in
    //to view a protected route; will redirect them to the login page;
    [Authorize]
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;
        private readonly IOwnerRepository _ownerRepo;

        public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepo = dogRepository;
            _ownerRepo = ownerRepository;
        }
        // GET: DogsController
        public ActionResult Index()
        {
            //setting ownerId variable to the current id of the owner/user that's logged in (using the private method we declared at the bottom of the page)
            int ownerId = GetCurrentUserId();
            //now passing in that ownerId as an argument to the GetDogsByOwnerId method that will get all the dogs for that owner/user;
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // GET: DogsController/Create
        public ActionResult Create()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            //using view model so we can connect the list of owners with the individual dog
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = new Dog(),
                Owners = owners
               
            };

            return View(vm);
        }

        // POST: DogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                //add a new dog to the database
                //this new dog may or may not have a value for Notes and/or ImageUrl
                //since we are not allowing user to chose the owner when adding a dog, we need to set the OwnerId to the id of the user/owner that is signed in;
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                
                return RedirectToAction("Index");
            }
            catch
            {
                List<Owner> owners = _ownerRepo.GetAllOwners();
                //if something goes wrong we return to the view, which is the DogFormViewModel
                DogFormViewModel vm = new DogFormViewModel()
                {
                    Dog = dog,
                    Owners = owners
                   
                };

                return View(vm);
            }
        }

        // GET: DogsController/Edit/5
        public ActionResult Edit(int id)
        {
           
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();
            List<Owner> owners = _ownerRepo.GetAllOwners();

            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = dog,
                Owners = owners
            };

           
            if (dog.OwnerId != ownerId)
            {
              return NotFound();
            }
            else 
            {
              return View(vm);
            }
            
        }

        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                
                _dogRepo.UpdateDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                DogFormViewModel vm = new DogFormViewModel()
                {
                    Dog = dog,
                    Owners = _ownerRepo.GetAllOwners()
                };

                return View(vm);
            }
        }

        // GET: DogsController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();
            if (dog.OwnerId == ownerId)
            {
                return View(dog);
            }
            else
            {
                return NotFound();
            }
            
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }

        //this is a private method that will get the id of the current user/owner that's logged in
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
