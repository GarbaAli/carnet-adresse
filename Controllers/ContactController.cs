  using carnet_adresse.Models;
using carnet_adresse.Repositories;
using carnet_adresse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Controllers
{
    
    public class ContactController : Controller
    {
        private readonly IContactRepository _bd;
        private readonly IWebHostEnvironment _hostingEnironment;
        public ContactController(IContactRepository Icontactrepository, IWebHostEnvironment hostingEnironment)
        {
            _bd = Icontactrepository;
            _hostingEnironment = hostingEnironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            //ContactViewModel contactVm = new ContactViewModel();
            //contactVm.contact = _bd.GetAllContact();
            //return View(contactVm);

            var ListContact = _bd.GetAllContact();
            return View(ListContact);
        }

        //[AllowAnonymous]
        public IActionResult Details(int id)
        {
            ContactViewModel contactVm = new ContactViewModel();
            contactVm.contact = _bd.GetContact(id);
            return View(contactVm);
        }

        [HttpGet]
        //[Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult Create(Contact contact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _bd.CreateContact(contact);
        //        return RedirectToAction("Details", new { id = contact.contactId });
        //    }
        //    return View();
        //}

        
        public IActionResult Create(ContactCreateViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.avatar != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnironment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid() + "_" + model.avatar.FileName;
                    string path = Path.Combine(uploadsFolder, uniqueFileName);
                    model.avatar.CopyTo(new FileStream(path, FileMode.Create));
                }

                Contact contact = new Contact()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    email = model.email,
                    phone = model.phone,
                    Bio = model.Bio,
                    avatar = uniqueFileName
                };
                _bd.CreateContact(contact);
                return RedirectToAction("Details", new { id = contact.contactId });
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                _bd.DeleteContact(id);
                return View("index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Update(int id)
        {
            Contact ct = _bd.GetContact(id);
            return View(ct);
        }

        [HttpPost]
        public IActionResult Update(ContactCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = _bd.GetContact(model.contactId);
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.email = model.email;
                contact.phone = model.phone;
                contact.Bio = model.Bio;

                string uniqueFileName = null;
                if (model.avatar != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnironment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid() + "_" + model.avatar.FileName;
                    string path = Path.Combine(uploadsFolder, uniqueFileName);
                    model.avatar.CopyTo(new FileStream(path, FileMode.Create));

                    if (contact.avatar != null)
                    {
                        //supprimmer l'ancien image
                        string ancienAvatar = Path.Combine(_hostingEnironment.WebRootPath, "Images", contact.avatar);
                        System.IO.File.Delete(ancienAvatar);
                    }
                }
                contact.avatar = uniqueFileName;

                _bd.UpdateContact(contact);
                return RedirectToAction("Details", new { id = model.contactId });
            }
            return View();
        }

    }
}
