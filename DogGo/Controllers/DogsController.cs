using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
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
            //using view model so we can connect the list of owners with the individual dog
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = new Dog(),
                Owners = _ownerRepo.GetAllOwners()
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
                _dogRepo.AddDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                //if something goes wrong we return to the view, which is the DogFormViewModel
                DogFormViewModel vm = new DogFormViewModel()
                {
                    Dog = dog,
                    Owners = _ownerRepo.GetAllOwners()
                };

                return View(vm);
            }
        }

        // GET: DogsController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            List<Owner> owners = _ownerRepo.GetAllOwners();

            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = dog,
                Owners = owners
            };

            return View(vm);
            //if (dog == null)
            //{
            //    return NotFound();
            //}
            //return View(dog);
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
            return View(dog);
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
