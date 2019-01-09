﻿using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Views;

namespace Controllers
{

    public class DataBaseController
    {
        //maak een nieuwe rol aan
        public void Add_Role(string roleName)
        {

            using (DataBase context = new DataBase())
            {

                var role = new Models.Role
                {
                    RoleName = roleName,
                    CreatedAt = DateTime.Now,

                };

                context.Roles.Add(role);

                context.SaveChanges();

            }

        }
        // maak een nieuwe diploma aan
        public void Add_Diploma(string diplomaName)
        {
            using (DataBase context = new DataBase())
            {
                var diploma = new Models.Diploma
                {
                    DiplomaName = diplomaName,
                };
                context.Diplomas.Add(diploma);
                context.SaveChanges();
            }
        }

        // voeg een nieuwe user diploma toe
        public void Add_UserDiploma(int diplomaID, int userID)
        {
            using (DataBase context = new DataBase())
            {
                var userDiploma = new Models.User_Diploma
                {
                    DiplomaID = diplomaID,
                    UserID = userID,
                    CreatedAt = DateTime.Now,

                };
                context.User_Diplomas.Add(userDiploma);
                context.SaveChanges();
            }
        }
        // voeg een nieuwe rol toe
        public void Add_UserRole(int roleID, int userID)
        {

            using (DataBase context = new DataBase())
            {
                var UserRole = new Models.User_Role
                {
                    RoleID = roleID,
                    UserID = userID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    DeletedAt = null
                };

                context.User_Roles.Add(UserRole);
                context.SaveChanges();


                var isEditedUserLoggedIn = LoginView.UserId != null && LoginView.UserId == userID;
                if (isEditedUserLoggedIn)
                {
                    Switcher.DeleteMenu();
                    Switcher.MenuMaker();
                }
            }
        }
        // haal alle rollen op
        public bool Get_UserRole(int roleID, int userID)
        {
            try
            {
                using (DataBase context = new DataBase())
                {
                    bool hasUserRole = context.User_Roles.Any(cus => cus.User.UserID == userID && cus.Role.RoleID == roleID);
                    return hasUserRole;

                }

            }
            catch (NullReferenceException)
            { return false; }

        }
        // verwijder gebruikers rol
        public void Delete_UserRole(int userID, int rolID)
        {
            using (DataBase context = new DataBase())
            {

                var deleteUserRole = (from x in context.User_Roles
                                      where x.User.UserID == userID && x.DeletedAt == null && x.Role.RoleID == rolID
                                      select x).ToList();




                context.User_Roles.RemoveRange(deleteUserRole);

                context.SaveChanges();
                Switcher.DeleteMenu();
                Switcher.MenuMaker();
            }

        }
        // verwijder diploma
        public void Delete_UserDiploma(int userID, int diplomaID)

        {
            using (DataBase context = new DataBase())
            {

                var delUserDiploma = (from x in context.User_Diplomas
                                      where x.User.UserID == userID && x.DeletedAt == null && x.Diploma.DiplomaID == diplomaID
                                      select x).ToList();


                context.User_Diplomas.RemoveRange(delUserDiploma);


                context.SaveChanges();

            }

        }
        //maak een nieuwe user aan
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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    DeletedAt = null

                };
                int UserID = user.UserID;
                context.Users.Add(user);


                context.SaveChanges();
            }

        }
        // verwijder user
        public static void Delete_User(int userID)
        {
            using (DataBase context = new DataBase())
            {

                Models.User delUser = context.Users.Where(d => d.UserID == userID).First();

                if (delUser != null)
                {

                    delUser.DeletedAt = DateTime.Now;

                    context.SaveChanges();

                    string sendMessage = $"Beste {delUser.Firstname},{Environment.NewLine}{Environment.NewLine}Uw lidmaatschap is vanaf vandaag opgezegd.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De Roeivereniging";
                    EmailController mail = new EmailController(delUser.Email, "Einde lidmaatschap", sendMessage);
                }
            }

        }
        //bewerk user
        public void Update_User(int userID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User dep = context.Users.Where(d => d.UserID == userID).First();


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
                    dep.UpdatedAt = DateTime.Now;
                    dep.DeletedAt = null;

                    context.SaveChanges();
                }

            }

        }

        //bewerk user
        public void Update_User(int userID, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (DataBase context = new DataBase())
            {

                Models.User dep = context.Users.Where(d => d.UserID == userID).First();


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
                    dep.UpdatedAt = DateTime.Now;
                    dep.DeletedAt = null;


                    context.SaveChanges();
                }


            }

        }

        // password hash
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

        //haal gebruiker op
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
        // print een lijst met gebruikers
        public void Print()
        {
            using (DataBase context = new DataBase())
            {
                // Display all courses from the database
                var users = (from s in context.Users
                             orderby s.UserID
                             select s).ToList<Models.User>();


                foreach (var boat in users)
                {

                    Console.WriteLine("ID: {0}, Voornaam: {1}, Achternaam: {2}, Address: {3}, Postcode: {4}, plaats: {5}, Telefoonnummer: {6}, Email: {7}", boat.UserID, boat.Firstname, boat.Lastname, boat.Address, boat.Zipcode, boat.City, boat.Phonenumber, boat.Email);
                }
                Console.ReadKey();
            }
        }


    }
}