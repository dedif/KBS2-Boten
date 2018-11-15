using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootRegistratieSysteem
{
    class DataBaseController
    {

        //public void EmptyDatabase()
        //{
        //    using (BootDataBase context = new BootDataBase())
        //    {
        //        context.Database.Delete();
        //    }
        //}
  
        public void Add_User(string password, string firstname, string lastname, string address, string zipcode, string city, string phonenumber, string email)
        {
            using (BootDataBase context = new BootDataBase())
            {



                var user = new User
                {
                    Password = password,
                    Firstname = firstname,
                    Lastname = lastname,
                    Address = address,
                    Zipcode = zipcode,
                    City = city,
                    Phonenumber = phonenumber,
                    Email = email
                  
                };

                context.Users.Add(user);


                context.SaveChanges();



              

            }

        }

        public void Print()
        {
            using (BootDataBase context = new BootDataBase())
            {
                // Display all courses from the database
                var users = (from s in context.Users
                             orderby s.PersonID
                             select s).ToList<User>();


                foreach (var boat in users)
                {

                    Console.WriteLine("ID: {0}, Voornaam: {1}, Achternaam: {2}, Address: {3}, Postcode: {4}, plaats: {5}, Telefoonnummer: {6}, Email: {7}", boat.PersonID, boat.Firstname, boat.Lastname, boat.Address, boat.Zipcode, boat.City, boat.Phonenumber, boat.Email);
                }
                Console.ReadKey();
            }
        }
      

    }
}
