using BataviaReseveringsSysteem.Database;
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


    }
}