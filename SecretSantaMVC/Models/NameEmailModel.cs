using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SecretSantaMVC.Models
{
    /*
     * Class used to create name-email pairs, 
     * intended to receive data from the secret
     * santa form.
     */
    public class NameEmailModel
    {
        public string[] Name { get; set; }
        public string[] Email { get; set; }
    }
}
