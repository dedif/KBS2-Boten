using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BootRegistratieSysteem;
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

        public void Add_User(string password, string firstname, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID,DateTime birthday)
        {
            using (BootDataBase context = new BootDataBase())
            {



                var user = new Model.User
                {
                    Password = password,
                    Firstname = firstname,
                    Lastname = lastname,
                    Address = address,
                    Zipcode = zipcode,
                    City = city,
                    Phonenumber = phonenumber,
                    Email = email,
                    GenderID = genderID,
                    Birthday = birthday
                };

                context.Users.Add(user);


                context.SaveChanges();





            }

        }


        public string PasswordHash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public void GetUser()
        {
            using (BootDataBase context = new BootDataBase())
            {
                var result = context.Users.ToList();

                foreach (var results in result)
                {
                    Console.WriteLine(results);
                }
            }

        }

        public void Print()
        {
            using (BootDataBase context = new BootDataBase())
            {
                // Display all courses from the database
                var users = (from s in context.Users
                             orderby s.PersonID
                             select s).ToList<Model.User>();


                foreach (var boat in users)
                {

                    Console.WriteLine("ID: {0}, Voornaam: {1}, Achternaam: {2}, Address: {3}, Postcode: {4}, plaats: {5}, Telefoonnummer: {6}, Email: {7}", boat.PersonID, boat.Firstname, boat.Lastname, boat.Address, boat.Zipcode, boat.City, boat.Phonenumber, boat.Email);
                }
                Console.ReadKey();
            }
        }


    }
}
