using BC.Intranet.Models;
using Core.Entities;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Controllers
{
    public class PlaceController : BaseController
    {
        internal BillionCompanyDbContext _context = new BillionCompanyDbContext();
        // GET: Place
        public ActionResult Index()
        {
            var model = new PlaceListViewModel();
            model.PageTitle = "List of places - Mowido";
            model.Header = "List of places";
            model.Places = _context.PlaceRepository.GetAll().ToList();
            return View(model);
        }

        public ActionResult CreatePlace()
        {
            var model = new PlaceViewModel();
            model.PageTitle = "Add place - Mowido";

            var placeTypes = _context.PlaceTypeRepository.GetAll().ToList();
            var parentPlaces = _context.PlaceRepository.GetAll().Where(p => p.PlaceTypeId != 4).ToList();

            model.PlaceTypes = new SelectList(placeTypes, "Id", "Name");
            model.ParentPlaces = new SelectList(parentPlaces, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePlace(PlaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newPlace = new Place();

                newPlace.Name = model.Name;
                newPlace.NameSV = model.NameSV;
                newPlace.PlaceTypeId = model.PlaceTypeId;
                newPlace.ParentId = model.ParentId;

                _context.PlaceRepository.Insert(newPlace);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult EditPlace(int id)
        {
            var model = new PlaceViewModel();
            model.PageTitle = "Edit place - Mowido";

            var place = _context.PlaceRepository.GetByID(id);

            model.Id = id;
            model.Name = place.Name;
            model.NameSV = place.NameSV;
            model.PlaceTypeId = place.PlaceTypeId;
            model.ParentId = place.ParentId;

            var placeTypes = _context.PlaceTypeRepository.GetAll().ToList();
            var parentPlaces = _context.PlaceRepository.GetAll().Where(p => p.PlaceTypeId != 4).ToList();

            model.PlaceTypes = new SelectList(placeTypes, "Id", "Name");
            model.ParentPlaces = new SelectList(parentPlaces, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPlace(PlaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var place = _context.PlaceRepository.GetByID(model.Id);

                place.Name = model.Name;
                place.NameSV = model.NameSV;
                place.ParentId = model.ParentId;
                place.PlaceTypeId = model.PlaceTypeId;

                _context.PlaceRepository.Update(place);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
    }
}