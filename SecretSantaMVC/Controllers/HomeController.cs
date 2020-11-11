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

        // POST: CommentModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatePublished,Title,Author,Text")] CommentModel commentModel)
        {
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
         * Index action for displaying view of home page.
         * AccessDB.GetComments() method is called, which returns a list
         * of strings. Each list represents a full comment. Although the
         * database is accessed successfully (the GetComments method outputs
         * each string to the console as it is accessed), it does not copy
         * over to here. So, although there is a script to render comments
         * in the view, no comments render. 
         */
        // GET: /Home/
        /*
         public ActionResult Index()
         {


             // GetComments returns a list of string lists. These now
             // need to be converted into a list of FullCommentModel objects.
             List<List<string>> allComments = AccessDB.GetComments();

             var allCommentsModel = new List<FullCommentModel>();



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

                 allCommentsModel.Add(fullCommentModel);

             }
             // Use a ViewBag to post comments to View
             ViewBag.AllCommentsModel = allCommentsModel;



             return View();

         }
        

        // Action for posting a comment (to be called whenever comments form is submitted)
        // This now works.

        [HttpPost]
        public ActionResult PostComment(ContainerModel containerModel)
        {
            var model = containerModel.CommentModel;
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
       
        */
        /*
         * Need a way to post each name/email pair here from the 
         * view so that I can convert them into a list of 
         * NameEmailModel objects, create a new SecretSanta object
         * from the list, and call its execute method to send out the
         * emails. 
         */
        [HttpPost]
        public ActionResult SendEmail(ContainerModel containerModel)
        //public ActionResult SendEmail(IFormCollection namesAndEmails)
        {
            var Text = containerModel.NameEmailModel.Name;
            var Email = containerModel.NameEmailModel.Email;

            for (int i = 0; i < Text.Length; i++)
            {
                Console.WriteLine("Text = " + Text[i]);
                Console.WriteLine("Email = " + Email[i]);
            }
            
           

            return RedirectToAction("Index");
        }
    }
}