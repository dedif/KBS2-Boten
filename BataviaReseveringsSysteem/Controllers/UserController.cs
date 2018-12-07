using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Controllers
{
    class UserController
    {
        
        //Maak een nieuwe gebruiker aan
        public void Add_User(string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {
                var user = new Models.User
                {

                    Password = password,
                    Firstname = firstname,
                    Lastname = lastname,
                    Middlename = middlename,
                    Address = address,
                    Zipcode = zipcode,
                    City = city,
                    Phonenumber = phonenumber,
                    Email = email,
                    GenderID = genderID,
                    Birthday = birthday,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    DeletedAt = null

                };
                int UserID = user.UserID;
                context.Users.Add(user);


                context.SaveChanges();
            }

        }

        //Bewerk een gebruiker met wachtwoord
        public void Update_User(int userID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User update = context.Users.Where(d => d.UserID == userID).First();
                if (update != null)
                {
                    update.Password = password;
                    update.Firstname = firstname;
                    update.Lastname = lastname;
                    update.Address = address;
                    update.Zipcode = zipcode;
                    update.City = city;
                    update.Phonenumber = phonenumber;
                    update.Email = email;
                    update.GenderID = genderID;
                    update.Birthday = birthday;
                    update.Middlename = middlename;
                    update.UpdatedAt= DateTime.Now;
                    update.DeletedAt = null;
                    
                    context.SaveChanges();
                }
            }

        }

        //Bewerk een gebruiker zonder wachtwoord
        public void Update_User(int userID, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User update = context.Users.Where(d => d.UserID == userID).First();
                if (update != null)
                {
                    update.Firstname = firstname;
                    update.Lastname = lastname;
                    update.Address = address;
                    update.Zipcode = zipcode;
                    update.City = city;
                    update.Phonenumber = phonenumber;
                    update.Email = email;
                    update.GenderID = genderID;
                    update.Birthday = birthday;
                    update.Middlename = middlename;
                    update.UpdatedAt = DateTime.Now;
                    update.DeletedAt = null;

                    context.SaveChanges();
                }
            }
        }

        //verwijder een gebruiker uit de userlist 
        public void Delete_User(int userID)
        {
            using (DataBase context = new DataBase())
            {

                Models.User delUser = context.Users.Where(d => d.UserID == userID).First();

                if (delUser != null)
                {

                    delUser.DeletedAt = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }


        //Hash een gebruikers wachtwoord
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

        // Get een gebruiker
        public void GetUser()
        {
            using (DataBase context = new DataBase())
            {
                var result = context.Users.ToList();

                foreach (var results in result)
                {
                    Console.WriteLine(results);
                }
            }

        }

        //Get een gebruikers ID
        public int GetID()
        {
            using (DataBase context = new DataBase())
            {

                var Id =(
                        from data in context.Users
                        orderby data.UserID descending
                        select data.UserID).First();
                return Id;
            }

        }


        public void Print()
        {
            using (DataBase context = new DataBase())
            {
                // Display all courses from the database
                var users = (from s in context.Users
                             orderby s.UserID
                             select s).ToList<User>();


                foreach (var boat in users)
                {

                    Console.WriteLine("ID: {0}, Voornaam: {1}, Achternaam: {2}, Address: {3}, Postcode: {4}, plaats: {5}, Telefoonnummer: {6}, Email: {7}", boat.UserID, boat.Firstname, boat.Lastname, boat.Address, boat.Zipcode, boat.City, boat.Phonenumber, boat.Email);
                }
                Console.ReadKey();
            }
        }


    }
}
