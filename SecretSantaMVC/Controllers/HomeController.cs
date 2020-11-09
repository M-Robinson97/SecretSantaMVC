using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSantaMVC.Models;

namespace SecretSantaMVC.Controllers

{
    public class HomeController : Controller
    {
        /*
         * Index action for displaying view of home page.
         * AccessDB.GetComments() method is called, which returns a list
         * of strings. Each list represents a full comment. Although the
         * database is accessed successfully (the GetComments method outputs
         * each string to the console as it is accessed), it does not copy
         * over to here. So, although there is a script to render comments
         * in the view, no comments render. 
         */
        // GET: /Home/
        public ActionResult Index()
        {

            // GetComments returns a list of string lists. These now
            // need to be converted into a list of FullCommentModel objects.
            List<List<string>> allComments = AccessDB.GetComments();
            
            var model = new List<FullCommentModel>();

            foreach (List<string> singleComment in allComments)
            {
                // singleComment[0] is ID, which we won't use
                string title = singleComment[1];
                string datePublished = singleComment[2];
                string author = singleComment[3];
                string text = singleComment[4];

                var fullCommentModel = new FullCommentModel
                {
                    Title = title,
                    DatePublished = datePublished,
                    Author = author,
                    Text = text
                };

                model.Add(fullCommentModel);

            }
            
            return View(model);

        }

        // Action for posting a comment (to be called whenever comments form is submitted)
        
        [HttpPost]
        public ActionResult PostComment(CommentModel model)
        {
            string title = model.Title;
            string author = model.Author;
            string text = model.Text;

            // PostComment takes care of all database admin for posting.
            AccessDB.PostComment(title, author, text);
            // GetComments prints entire contents of Comments table to console -
            // Used to test the a comment has posed.
            AccessDB.GetComments();

            return RedirectToAction("Index");
        }
       

        /*
         * Need a way to post each name/email pair here from the 
         * view so that I can convert them into a list of 
         * NameEmailModel objects, create a new SecretSanta object
         * from the list, and call its execute method to send out the
         * emails. 
         */
        [HttpPost]
        public ActionResult SendEmail(CommentModel model)
        {
            string name = model.Title;
            string email = model.Author;
            

            Console.WriteLine("name: " + name);
            Console.WriteLine("email: " + email);

            return RedirectToAction("Index");
        }
    }
}