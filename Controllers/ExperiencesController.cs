﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IsabellaBirdBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IsabellaBirdBlog.Controllers
{
    public class ExperiencesController : Controller
    {
        private IsabellaBirdBlogContext db = new IsabellaBirdBlogContext();
        
        public IActionResult Index()
        {
            return View(db.Experiences.ToList());
        }

        public IActionResult Details(int id)
        {
            var thisExperience = db.Experiences
                .Include(experiences => experiences.Location)
                .FirstOrDefault(experiences => experiences.ExperienceId == id);

            ViewBag.People = db.Experiences
                .Include(experience => experience.PersonsExperiences)
                .ThenInclude(personsExperiences => personsExperiences.Person)
                .Where(experience => experience.ExperienceId == id).ToList();
            return View(thisExperience);
        }

        public IActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", "GeoClass");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Experience experience)
        {
            db.Experiences.Add(experience);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var thisExperience = db.Experiences
                .Include(experiences => experiences.Location)
                .FirstOrDefault(experiences => experiences.ExperienceId == id);

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", "GeoClass");

            return View(thisExperience);
        }

        [HttpPost]
        public IActionResult Edit(Experience experience)
        {
            db.Entry(experience).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
