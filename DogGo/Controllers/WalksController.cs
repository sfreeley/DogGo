using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(IDogRepository dogRepository, IWalkerRepository walkerRepository, IWalkRepository walkRepository)
        {

            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;


        }
        // GET: WalksController
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create()
        { 
            //getting list of all the walkers 
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            //getting list of all the dogs
            List<Dog> dogs = _dogRepo.GetAllDogs();
            //this will be a list of all the dogs (iterating through the dogs list and adding them to the listSelectListItem )
            List<SelectListItem> listSelectListItem = new List<SelectListItem>();
            foreach (Dog dog in dogs)
            {
                //with each iteration we are setting the properties of this SelectListItem object
                //and adding each object to the list
                //Text is a property of the SelectListItem class (display text of the selected item)
                //Value is a property of SelectListItem class (value of the selected item- int needs to be converted to string)
                //Selected is a property of SelectListItem class (value that indicated whether this SelectListItem has been selected - boolean on the Dog class)
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = dog.Name,
                    Value = dog.Id.ToString(),
                    Selected = dog.isSelected
                };
                //with each iteration each item with these properties will be added to the list of dogs
                listSelectListItem.Add(selectListItem);
            }

            //instanstiate the viewmodel and set the properties of the viewmodel to the proper values; DogsList will be the listSelectListItem that was 
            //formed with each addition of the dog to the list
            WalkFormViewModel vm = new WalkFormViewModel()
            {
                Walk = new Walk(),
                Dogs = dogs,
                Walkers = walkers,
                DogsList = listSelectListItem
            };
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //this is the POST functionality in the Create method for a walk, which will take in the selectedDogs Ids as strings and the view model
        public ActionResult Create(IEnumerable<string> selectedDogs, WalkFormViewModel walkform)
        {
            try
            {
                foreach (string idString in selectedDogs)
                {

                    walkform.Walk.DogId = int.Parse(idString);
                    _walkRepo.AddWalk(walkform.Walk);

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //if something goes wrong we return to the view, which is the WalkFormViewModel
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                List<Dog> dogs = _dogRepo.GetAllDogs();
                List<SelectListItem> listSelectListItem = new List<SelectListItem>();
                WalkFormViewModel vm = new WalkFormViewModel()
                {
                    Walk = new Walk(),
                    Dogs = dogs,
                    Walkers = walkers,
                    DogsList = listSelectListItem
                };
                return View(vm);

            }
            
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
