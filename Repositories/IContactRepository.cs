using carnet_adresse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Repositories
{
   public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContact();

        Contact GetContact(int id);

        void UpdateContact(Contact contact);
        void CreateContact(Contact contact);
        void DeleteContact(int id);

    }
}
