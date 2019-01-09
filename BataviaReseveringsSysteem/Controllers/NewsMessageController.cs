using BataviaReseveringsSysteem.Database;
using System;
using System.Linq;

namespace BataviaReseveringsSysteem.Controllers
{
    public class NewsMessageController
    {
        private string notification;

        public string Notification()
        {
            return notification;
        }
        // check of de velden leeg zijn
        public Boolean WhiteCheck(string title, string message)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
            {
                notification = "U hebt niet alle velden ingevuld";

                return false;
            }
            else
            {
                notification = "";
                return true;
            }
        }
        // maak een nieuw nieuwsbericht aan
        public void Add_NewsMessage(int userID, string title, string message)
        {
            using (DataBase context = new DataBase())
            {
                var NewsMessage = new Models.News_Message
                {
                    UserID = userID,
                    Title = title,
                    Message = message,
                    CreatedAt = DateTime.Now,
                };
                context.News_Messages.Add(NewsMessage);
                context.SaveChanges();
            }
        }
        // bewerk een bestaand nieuwsbericht
        public void Update_NewsMessage(int newsMessageID,int userID, string title, string message)
        {
            using (DataBase context = new DataBase())
            {
                Models.News_Message newsMessage = context.News_Messages.Where(d => d.NewsMessageID == newsMessageID).First();
                if (newsMessage != null)
                {
                    newsMessage.Title = title;
                    newsMessage.UserID = userID;
                    newsMessage.Message = message;
                    newsMessage.UpdatedAt = DateTime.Now;
                    newsMessage.DeletedAt = null;
                    context.SaveChanges();
                }
            }
        }
        // verwijder een nieuwsbericht
        public void Delete_NewsMessage(int newsMessageID)
        {
            using (DataBase context = new DataBase())
            {
                Models.News_Message newsMessage = context.News_Messages.Where(d => d.NewsMessageID == newsMessageID).First();
                if (newsMessage != null)
                {
                    newsMessage.DeletedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
    }
}
