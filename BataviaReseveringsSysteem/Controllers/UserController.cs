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
        public void Add_User(string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday,DateTime? endOfSub)
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
                    DeletedAt = null,
                    EndOfSubscription = endOfSub

                };
                int UserID = user.UserID;
                context.Users.Add(user);


                context.SaveChanges();
            }

           
        }

        public void Add_Gender(string GenderName)
        {
            using (DataBase context = new DataBase())
            {
                var gender = new Models.Gender
                {

                    GenderName = GenderName,
                  

                };
                context.Genders.Add(gender);


                context.SaveChanges();
            }

        }

        //Bewerk een gebruiker met wachtwoord
        public void Update_User(int userID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday,DateTime? endOfSub)
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
                    update.EndOfSubscription = endOfSub;
                    
                    context.SaveChanges();
                }
            }

        }

        //Bewerk een gebruiker zonder wachtwoord
        public void Update_User(int userID, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday,DateTime? endOfSub)
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
                    update.EndOfSubscription = endOfSub;
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

                string sendMessage = $"Beste {delUser.Firstname},{Environment.NewLine}{Environment.NewLine} Uw lidmaatschap is vanaf vandaag opgezegd.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De Roeivereniging";

                EmailController mail = new EmailController(
                    delUser.Email,
                    "Abbonement", sendMessage);
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

                var Id = (
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

        public List<Role> GetRolesFromLoggedInUser()
        {
            using (var context = new DataBase())
                return (from user in context.Users
                        where user.UserID == LoginView.UserId
                        join userRole in context.User_Roles on user.UserID equals userRole.UserID
                        join role in context.Roles on userRole.RoleID equals role.RoleID
                        select role).ToList();
        }

        public bool LoggedInUserIsRaceCommissioner() =>
            GetRolesFromLoggedInUser().Any(role => role.RoleName.Equals("Wedstrijd Commissaris"));

        public static void CheckSubscription()
        {
            using (DataBase context = new DataBase())
            {
                var users = (from s in context.Users
                             where s.EndOfSubscription != null
                             where s.DeletedAt == null
                             select s).ToList<User>();
                if (users != null)
                {


                    foreach (var user in users)
                    {
                        if (user.EndOfSubscription <= DateTime.Now)
                        {
                            string sendMessage = $"Beste {user.Firstname},{Environment.NewLine}{Environment.NewLine} Uw lidmaatschap is vanaf vandaag verlopen.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De Roeivereniging";

                            EmailController mail = new EmailController(
                                user.Email,
                                "Einde lidmaatschap", sendMessage);

                            User dep = context.Users.Where(d => d.UserID == user.UserID).First();
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
                    }
                    context.SaveChanges();
                }
            }
        }

        public bool DataBaseContainsManagementUser()
        {
            var now = DateTime.Now;
            using (var context = new DataBase())
                return
                    (from userRole in context.User_Roles
                        where userRole.User.DeletedAt == null || userRole.User.DeletedAt <= now
                        where userRole.Role.DeletedAt == null || userRole.Role.DeletedAt <= now
                        where userRole.DeletedAt == null || userRole.DeletedAt <= now
                        where userRole.Role.RoleName.Equals("Bestuur")
                        select userRole).Any();
        }
    }
}
