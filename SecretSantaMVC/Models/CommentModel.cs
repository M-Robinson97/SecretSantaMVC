using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSantaMVC.Models
{
    /*
     * Class to create CommentModel objects
     * to post to the database.
     * Great tutorial for connecting via Entity:
     * https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-5.0&tabs=visual-studio
     */
    public class CommentModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }
        public string Title { get; set; }
        
        public string Author { get; set; }
        public string Text { get; set; }

    }
}
