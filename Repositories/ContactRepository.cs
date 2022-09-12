using carnet_adresse.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;
        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateContact(Contact contact)
        {
             _context.contacts.Add(contact);
            _context.SaveChanges();

        }

        public void DeleteContact(int id)
        {
            var ct = GetContact(id);
            if (ct != null)
            {
                _context.contacts.Remove(ct);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Contact> GetAllContact()
        {
            return _context.contacts.Include(c => c.Adresses).ToList();
        }

        public Contact GetContact(int id)
        {
            return _context.contacts.Include(c => c.Adresses).SingleOrDefault(ct => ct.contactId == id);
        }

        public void UpdateContact(Contact contact)
        {
            _context.contacts.Update(contact);
            _context.SaveChanges();
        }
    }
}
