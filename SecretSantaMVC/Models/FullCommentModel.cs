using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSantaMVC.Models
{
    /*
     * Class for creating FullCommentModel objects to retrieve
     * from the database.
     */
    public class FullCommentModel
    { 
        public string Title { get; set; }

        public string DatePublished { get; set; }

        public string Author { get; set; }
        public string Text { get; set; }
    }
}
