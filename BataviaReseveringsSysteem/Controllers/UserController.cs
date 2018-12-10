using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Views;


namespace Controllers
{
    class UserController
    {

        public void Add_User(string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                // Use current timeDateTime dt = 

                string mySqlTimestamp = "01-01-1900 00:00:00";
                DateTime time = DateTime.Parse(mySqlTimestamp);
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
                    Created_at = DateTime.Now,
                    Updated_at = null,
                    Deleted_at = null

                };
                int PersonID = user.PersonID;
                context.Users.Add(user);


                context.SaveChanges();
            }

        }

        public void Update_User(int personID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User dep = context.Users.Where(d => d.PersonID == personID).First();
                if (dep != null)
                {
                    dep.Password = password;
                    dep.Firstname = firstname;
                    dep.Lastname = lastname;
                    dep.Address = address;
                    dep.Zipcode = zipcode;
                    dep.City = city;
                    dep.Phonenumber = phonenumber;
                    dep.Email = email;
                    dep.GenderID = genderID;
                    dep.Birthday = birthday;
                    dep.Middlename = middlename;
                    dep.Updated_at = DateTime.Now;
                    dep.Deleted_at = null;

                    context.SaveChanges();
                }
            }

        }


        public void Update_User(int personID, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User dep = context.Users.Where(d => d.PersonID == personID).First();
                if (dep != null)
                {
                    dep.Firstname = firstname;
                    dep.Lastname = lastname;
                    dep.Address = address;
                    dep.Zipcode = zipcode;
                    dep.City = city;
                    dep.Phonenumber = phonenumber;
                    dep.Email = email;
                    dep.GenderID = genderID;
                    dep.Birthday = birthday;
                    dep.Middlename = middlename;
                    dep.Updated_at = DateTime.Now;
                    dep.Deleted_at = null;

                    context.SaveChanges();
                }
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
            using (DataBase context = new DataBase())
            {
                var result = context.Users.ToList();

                foreach (var results in result)
                {
                    Console.WriteLine(results);
                }
            }

        }

        public int GetID()
        {
            using (DataBase context = new DataBase())
            {

                var Id = (
                        from data in context.Users
                        orderby data.PersonID descending
                        select data.PersonID).First();
                return Id;
            }


        }


        public void Print()
        {
            using (DataBase context = new DataBase())
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

        public List<Role> GetRolesFromLoggedInUser()
        {
            using (var context = new DataBase())
                return (from user in context.Users
                        where user.PersonID == LoginView.UserId
                        join memberRole in context.MemberRoles on user.PersonID equals memberRole.PersonID
                        join role in context.Roles on memberRole.RoleID equals role.RoleID
                        select role).ToList();
        }

        public bool LoggedInUserIsRaceCommissioner() =>
            GetRolesFromLoggedInUser().Any(role => role.RoleName.Equals("Wedstrijd Commissaris"));
    }
}
