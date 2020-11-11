using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using SecretSantaMVC.Models;

/*
 * This namespace contains everything needed to execute the back-end Secret Santa function. There
 * are three classes. Class SecretSanta can be used to create SecretSanta objects storing 
 * pairs of NameEmail objects representing allocated givers and receivers. Shuffle class is a static
 * class used in the allocation process and its ShuffleNames method is called by the constructor of 
 * SecretSanta. SendEmail is a static class containing the static method SendNow, which sends out
 * an email. 
 * 
 * To send an email: simply create a SecretSanta object, and call its Execute() method.
 */
namespace ExecuteSanta
{
    /*
     * Class for creating pair of givers and receivers for Secret Santa.
     */
    public class SecretSanta
    {
        /*
         * Field variable storing pairs of givers and receivers.
         */
        public List<NameEmailModel[]> giveReceivePairs;
        /*
         * Constructor for creating SecretSanta objects. Takes a list
         * of email addresses as parameter. Throws an exception if there
         * are fewer than three. Otherwise, initialises giveReceivePairs
         * as the return of pairUp helper function. 
         */
        public SecretSanta(List<NameEmailModel> unsortedEmails)
        {
            if (unsortedEmails.Count < 3)
            {
                throw new System.ArgumentException("List cannot have fewer than three elements", "unsortedEmails");
            }
            this.giveReceivePairs = pairUp(unsortedEmails);
        }

        /*
         * Method pairing up givers and receivers with the following 
         * strategy:
         * - Shuffle the list of givers and receivers (using Shuffle method)
         * - Iterate through the shuffled list. For each email address:
         *    * Allocate that email as the giver
         *    * Allocate the next email as the receiver
         *    Until we reach the end of the list, where the receiver is
         *    set as the first email address of the list.
         */
        public List<NameEmailModel[]> pairUp(List<NameEmailModel> unsortedEmails)
        {
            List<NameEmailModel[]> sortedEmails = new List<NameEmailModel[]>();

            Shuffle.ShuffleNames(unsortedEmails);

            for (int i = 0; i < unsortedEmails.Count; i++)
            {

                NameEmailModel[] pairing = new NameEmailModel[2];
                if (i != (unsortedEmails.Count - 1))
                {
                    pairing[0] = unsortedEmails[i];
                    pairing[1] = unsortedEmails[i + 1];
                }
                else
                {
                    pairing[0] = unsortedEmails[i];
                    pairing[1] = unsortedEmails[0];
                }
                sortedEmails.Add(pairing);
            }
            return sortedEmails;
        }

        /*
         * Method for sending emails.
         */
        public void Execute()
        {
            foreach (NameEmailModel[] pair in giveReceivePairs)
            {
                //SendEmail.SendNow(pair);
            }
        }
    }

    /*
     * Shuffling strategy borrowed from:
     * https://www.programming-idioms.org/idiom/10/shuffle-a-list/1352/csharp
     * Moved to a separate static class from within SecretSanta for code reusability.
     */
    public static class Shuffle
    {
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
    }

    /*
     * Static class for sending emails. Again, method within has been
     * moved to increase code reusability. 
     */
    public static class SendEmail
    {
        /*
        public static void SendNow(NameEmailModel[] giveReceivePair)
        {
            string giverName = giveReceivePair[0].Name;
            string giverEmail = giveReceivePair[0].Email;
            string receiverName = giveReceivePair[1].Name;

            string senderEmail = "MarkRobinsonEmailTests@gmail.com";
            string senderPassword = "12fiDdle.!";
            string subject = "Your Secret Santa Allocation!";
            string body = $"Hello {giverName}, your Secret Santa allocation is {receiverName}! Merry Christmas!;";

            MailAddress from = new MailAddress(senderEmail);
            MailAddress to = new MailAddress(giverEmail);

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
            };

            try
            {
                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        */

    }
}
