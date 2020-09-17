using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        //private readonly IWalkerRepository _walkerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public OwnersController(IOwnerRepository ownerRepository, IDogRepository dogRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
        }
        // GET: OwnersController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnersController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);

            ProfileViewModel dogown = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs
            };
            return View(dogown);

            //previous owner details code
            //if (owner == null)
            //{
            //    return NotFound();
            //}

            //return View(owner);
        }

        // GET: OwnersController/Create
        //this method only creates the form and displays it to the user
        public ActionResult Create()
        {
            return View();
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
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
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
                return View(owner);
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
