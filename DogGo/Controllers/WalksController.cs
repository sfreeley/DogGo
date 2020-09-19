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
            
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            List<Dog> dogs = _dogRepo.GetAllDogs();
            List<SelectListItem> listSelectListItem = new List<SelectListItem>();
            foreach (Dog dog in dogs)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = dog.Name,
                    Value = dog.Id.ToString(),
                    Selected = dog.isSelected
                };
                listSelectListItem.Add(selectListItem);
            }

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
        public ActionResult Create(IEnumerable<string> selectedDogs, WalkFormViewModel walkform)
        {

            foreach (string idString in selectedDogs)
            {
                
                walkform.Walk.DogId = int.Parse(idString);
                _walkRepo.AddWalk(walkform.Walk);

            }



               
               
                return RedirectToAction(nameof(Index));
           
            
                //return View();
            
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
