using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSantaMVC.Models
{
    /*
     * Class to create CommentModel objects
     * to post to the database.
     */
    public class CommentModel
    {
       
        public string Title { get; set; }
        
        public string Author { get; set; }
        public string Text { get; set; }

    }
}
