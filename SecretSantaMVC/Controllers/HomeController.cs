using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSantaMVC.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecretSantaMVC.Data;
using System.Net;
using System.Net.Mail;

/*
 * Controller for the home page.
 */
namespace SecretSantaMVC.Controllers

{
    public class HomeController : Controller
    {
        private readonly SecretSantaMVCContext _context;

        public HomeController(SecretSantaMVCContext context)
        {
            // Uses dependency injection to inject database context into controller
            _context = context;
        }

        // GET: CommentModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.CommentModel.ToListAsync());
        }

        // POST: Home/CommentsForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatePublished,Title,Author,Text")] CommentModel commentModel)
        {
            // Set today as date of model
            DateTime today = DateTime.Now;
            commentModel.DatePublished = today;

            if (ModelState.IsValid)
            {
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }
        
        /*
         * Controller receiving input from the secret santa form. Framework binds each name and email
         * by adding them to the Name array and Email array of a NameEmail Model. This action processes
         * that data and emails each user to inform them who they need to buy a present for for secret santa.
         */
        // POST: Home/SecretSantaForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail([Bind("Name,Email")] NameEmailModel nameEmailModel)
        {
            List<NameEmailModel> nameEmails = new List<NameEmailModel>();

            for (int i = 0; i < nameEmailModel.Name.Length; i++)
            {
                NameEmailModel nameEmail = new NameEmailModel();
                nameEmail.Name = new string[] { nameEmailModel.Name[i] };
                Console.WriteLine("Name: " + nameEmailModel.Name[i]);
                nameEmail.Email = new string[] { nameEmailModel.Email[i] };
                Console.WriteLine("Email: " + nameEmailModel.Email[i]);

                nameEmails.Add(nameEmail);
            }
            // Call ShuffleNames helper (list of names and emails in random order)
            HomeControllerHelpers.ShuffleNames(nameEmails);
            // Call PairUp helper (assign each giver's receiver as the next person in the shuffled list)
            List<NameEmailModel> pairedNames = HomeControllerHelpers.PairUp(nameEmails);
            // Call SendNow helper (email all players)
            HomeControllerHelpers.SendNow(pairedNames);

            return RedirectToAction("Index");


        }
    }

    /*
     * Static class containing helper methods for HomeController class
     */

    public static class HomeControllerHelpers { 
        /*
         * Use a shuffling algorithm to randomise the order of the list of names
         */
        public static void ShuffleNames<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /*
         * Pair each person in the shuffled list with the next person in the shuffled list (except for
         * the last person in the shuffled list, who is paired with the first person)
         */
        public static List<NameEmailModel> PairUp(List<NameEmailModel> nameEmails)
        {
            List<NameEmailModel> sortedEmails = new List<NameEmailModel>();

            for (int i = 0; i < nameEmails.Count; i++)
            {
                NameEmailModel pairing = new NameEmailModel();

                if (i != (nameEmails.Count - 1))
                {
                    // Each person in the shuffled list is allocated the next person in the list
                    string[] namePair = new string[] { nameEmails[i].Name[0], nameEmails[i + 1].Name[0] };
                    string[] emailPair = new string[] { nameEmails[i].Email[0], nameEmails[i + 1].Email[0] };

                    pairing.Name = namePair;
                    pairing.Email = emailPair;
                }
                else
                {
                    // The last person in the list is allocated the first person in the list
                    string[] namePair = new string[] { nameEmails[i].Name[0], nameEmails[0].Name[0] };
                    string[] emailPair = new string[] { nameEmails[i].Email[0], nameEmails[0].Email[0] };

                    pairing.Name = namePair;
                    pairing.Email = emailPair;
                }
                sortedEmails.Add(pairing);
            }
            return sortedEmails;
        }

        /*
         * Send email from a gmail account set up for this purpose.
         */
        public static void SendNow(List<NameEmailModel> pairing)
        {
            string senderEmail = "MarkRobinsonEmailTests@gmail.com";
            string senderPassword = "12fiDdle.!";
            string subject = "Your Secret Santa Allocation!";

            for (int i = 0; i < pairing.Count; i++)
            {
                string giverName = pairing[i].Name[0];
                string giverEmail = pairing[i].Email[0];
                string receiverName = pairing[i].Name[1];
                string receiverEmail = pairing[i].Email[1];

                string body = $"Hello {giverName}, your Secret Santa allocation is {receiverName} (Email: {receiverEmail})! Merry Christmas!;";

                MailAddress sender = new MailAddress(senderEmail);
                Console.WriteLine("Giver email : " + giverEmail);
                MailAddress to = new MailAddress(giverEmail);



                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sender.Address, senderPassword),
                };

                using (var message = new MailMessage(sender, to)
                {
                    Subject = subject,
                    Body = body
                })


                try
                {
                    smtpClient.Send(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}