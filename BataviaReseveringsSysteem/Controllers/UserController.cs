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
        //Maak een nieuwe gebruiker aan
        public void Add_User(string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday, DateTime? endOfSub)
        {
            using (var context = new DataBase())
            {
                var user = new User
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
                    DeletedAt = null,
                    EndOfSubscription = endOfSub
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        // voeg een nieuw geslacht toe
        public void Add_Gender(string GenderName)
        {
            using (var context = new DataBase())
            {
                var gender = new Gender
                {
                    GenderName = GenderName,
                };
                context.Genders.Add(gender);
                context.SaveChanges();
            }
        }

        //Bewerk een gebruiker met wachtwoord
        public void Update_User(int userID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday, DateTime? endOfSub)
        {
            using (var context = new DataBase())
            {
                var update = context.Users.Where(d => d.UserID == userID).First();
                if (update == null) return;
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
                update.UpdatedAt = DateTime.Now;
                update.DeletedAt = null;
                update.EndOfSubscription = endOfSub;
                context.SaveChanges();
            }
        }

        //Bewerk een gebruiker zonder wachtwoord
        public void Update_User(int userID, string firstname, string middleName, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday, DateTime? endOfSub)
        {
            using (var context = new DataBase())
            {
                var update = context.Users.First(d => d.UserID == userID);
                if (update == null) return;
                update.Firstname = firstname;
                update.Lastname = lastname;
                update.Address = address;
                update.Zipcode = zipcode;
                update.City = city;
                update.Phonenumber = phonenumber;
                update.Email = email;
                update.GenderID = genderID;
                update.Birthday = birthday;
                update.Middlename = middleName;
                update.UpdatedAt = DateTime.Now;
                update.DeletedAt = null;
                update.EndOfSubscription = endOfSub;
                context.SaveChanges();
            }
        }

        //verwijder een gebruiker uit de userlist 
        public void Delete_User(int userID)
        {
            using (var context = new DataBase())
            {
                var delUser = context.Users.First(d => d.UserID == userID);
                if (delUser != null)
                {
                    delUser.DeletedAt = DateTime.Now;
                    context.SaveChanges();
                }
                var sendMessage = $"Beste {delUser.Firstname},{Environment.NewLine}{Environment.NewLine} Uw lidmaatschap is vanaf vandaag opgezegd.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De Roeivereniging";
                new EmailController(delUser.Email, "Abbonement", sendMessage);
            }
        }


        //Hash een gebruikers wachtwoord
        public string PasswordHash(string rawData)
        {
            // Create a SHA256   
            using (var sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                foreach (var t in bytes) builder.Append(t.ToString("x2"));
                return builder.ToString();
            }
        }

       
        //Get een gebruikers ID
        public int GetID()
        {
            using (var context = new DataBase())
                return (
                    from data in context.Users
                    orderby data.UserID descending
                    select data.UserID).First();
        }

     
        // haal alle gebruikersrollen op van de gebruiker
        public List<Role> GetRolesFromLoggedInUser()
        {
            using (var context = new DataBase())
                return (from user in context.Users
                        where user.UserID == LoginView.UserId
                        join userRole in context.User_Roles on user.UserID equals userRole.UserID
                        join role in context.Roles on userRole.RoleID equals role.RoleID
                        select role).ToList();
        }

        // kijk of iemand een subscription heeft en als die is geweest pas dan bepaalde velden aan voor privacy
        public static void CheckSubscription()
        {
            using (var context = new DataBase())
            {
                var users = (from s in context.Users
                             where s.EndOfSubscription != null
                             where s.DeletedAt == null
                             select s).ToList();
                if (users == null) return;
                foreach (var user in users)
                {
                    if (!(user.EndOfSubscription <= DateTime.Now)) continue;
                    var sendMessage = $"Beste {user.Firstname},{Environment.NewLine}{Environment.NewLine} Uw lidmaatschap is vanaf vandaag verlopen.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De Roeivereniging";
                    new EmailController(user.Email, "Einde lidmaatschap", sendMessage);
                    var dep = context.Users.First(d => d.UserID == user.UserID);
                    dep.City = null;
                    dep.Address = null;
                    dep.Birthday = DateTime.MaxValue;
                    dep.Gender = null;
                    dep.Password = null;
                    dep.Zipcode = null;
                    dep.Phonenumber = null;
                    dep.Email = null;
                    dep.DeletedAt = DateTime.Now;
                }
                context.SaveChanges();
            }
        }
        // kijken of er geen bestuursleden zijn die zijn verwijdert 
        public bool DataBaseContainsUndeletedManagementUser()
        {
            var now = DateTime.Now;
            using (var context = new DataBase())
                return
                    (from userRole in context.User_Roles
                     where userRole.User.DeletedAt == null || userRole.User.DeletedAt > now
                     where userRole.Role.DeletedAt == null || userRole.Role.DeletedAt > now
                     where userRole.DeletedAt == null || userRole.DeletedAt > now
                     where userRole.Role.RoleName.Equals("Bestuur")
                     select userRole).Any();
        }

        public bool LoggedInUserIsCoach() => GetRolesFromLoggedInUser().Any(role => role.RoleName.Equals("Coach"));
    }
}