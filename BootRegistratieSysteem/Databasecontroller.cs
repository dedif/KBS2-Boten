﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BootRegistratieSysteem;
namespace BootRegistratieSysteem
{
    class DataBaseController
    {
     
        public void Add_Role(string roleName)
        {
            
            using (BootDataBase context = new BootDataBase())
            {
               


                var role = new Models.Role
                {
                    RoleName = roleName,
                    Created_at = DateTime.Now,

                };

                context.Roles.Add(role);


                context.SaveChanges();





            }

        }

        public void Add_MemberRole(int roleID,int personID)
        {

            using (BootDataBase context = new BootDataBase())
            {



                var MemberRole = new Models.MemberRole
                {
                    RoleID = roleID,
                    PersonID = personID,
                    Created_at = DateTime.Now,
                    Updated_at = null,
                    Deleted_at = null

                };

                context.MemberRoles.Add(MemberRole);
                context.SaveChanges();
            }

        }

        public void Delete_MemberRole(int personID, int rolID)
        {
            using (BootDataBase context = new BootDataBase())
            {

                //Models.MemberRole delMemberRole = context.MemberRoles.Where(d => d.PersonID == personID).First();

                var delMemberRole = (from x in context.MemberRoles
                                     where x.PersonID == personID && x.Deleted_at == null && x.RoleID == rolID
                                     select x).ToList();


                //if (delMemberRole != null)
                //{
                //    delMemberRole.ForEach(x => x.Deleted_at = DateTime.Now);
            
                //}

                // if (delMemberRole != null)
                // {
                //   delMemberRole.Deleted_at = DateTime.Now;

                context.MemberRoles.RemoveRange(delMemberRole);

                context.SaveChanges();
               // }
            }

        }

        public void Add_User(string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID,DateTime birthday)
        {
            using (BootDataBase context = new BootDataBase())
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

                context.Users.Add(user);


                context.SaveChanges();





            }

        }

        public void Delete_User(int personID)
        {
            using (BootDataBase context = new BootDataBase())
            {

               Models.User delUser = context.Users.Where(d => d.PersonID == personID).First();

            
                if (delUser != null)
                {

                    delUser.Deleted_at = DateTime.Now;
                   
                    context.SaveChanges();
                }




            }

        }
        public void Update_User(int personID, string password, string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (BootDataBase context = new BootDataBase())
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


        public void Update_User(int personID,  string firstname, string middlename, string lastname, string address, string zipcode, string city, string phonenumber, string email, int genderID, DateTime birthday)
        {
            using (BootDataBase context = new BootDataBase())
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
                             select s).ToList<Models.User>();


                foreach (var boat in users)
                {

                    Console.WriteLine("ID: {0}, Voornaam: {1}, Achternaam: {2}, Address: {3}, Postcode: {4}, plaats: {5}, Telefoonnummer: {6}, Email: {7}", boat.PersonID, boat.Firstname, boat.Lastname, boat.Address, boat.Zipcode, boat.City, boat.Phonenumber, boat.Email);
                }
                Console.ReadKey();
            }
        }


    }
}
