using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Auth.Models;
using Auth.Services.Interfaces;

namespace Auth.Services
{
    public class MailService : IMailService
    {
        public void SendConfirmationEmailAsync(User user, Guid id, string domain)
        {
            string msg = "<div style=\"width: 100%; display: flex; align-items: center; justify-content: center; background-image: url(https://www.muralswallpaper.com/app/uploads/Green-Tropical-Plant-Wallpaper-Mural-Plain-820x532.jpg)\">"
                                + "<div style =\"width: 80%; display: flex; flex-direction: column; align-items: center; justify-content: center; font-family: sans-serif; font-weight: 600; color: #fff; background-color: rgba(0,0,0,.8); padding: 1em;\">"
                                        + "<h3> User registration request!</h3>"
                                        + "<div style =\"display: flex; flex-direction: column; align-items: center; justify-content: center;\">"
                                            + $"<p style =\"width: 80%; text-align: center;\">User <b>{user.UserName}</b> wants to create an account in CarPartsStore.</p>"
                                            + $"<p> Email: {user.Email}</p>"
                                            + "<p style =\"color: gray; font-weight: 700;\">Do you want to allow him/her create the account?</p>"
                                        + "</div>"
                                        + "<div style =\"width: 100%; display: flex; align-items: center; justify-content: center;\">"
                                            + "<div style =\"display: flex; flex-direction: row; align-items: center; justify-content: space-around; width: 30%;\">"
                                                + $"<a href =\"http://{domain}/api/account/ConfirmEmail?id={id}\" style=\"text-decoration: none; width: 90%; height: 8vh; background-color: #fed136; color: #fff; text-transform: uppercase; font-weight: 800; display: flex; align-items: center; justify-content: center; border-radius: 0.7em; border: 2px solid #fff; margin-right: 0.5em;\">" +
                                                     $"Confirm" +
                                                  $"</a>"
                                            + "</div>"
                                        + "</div>"
                                + "</div>"
                            + "</div>";
            var receiverEmail = new MailAddress(user.Email, user.UserName);
            var senderEmail = new MailAddress("carpartsstore.mailer@gmail.com", "Car Parts Store");
            var password = "CarPartsPwd34";
            var subject = "Confirm user registration.";
            var body = msg;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };

            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }
    }
}
