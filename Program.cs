using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace AssessmentTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName, senderEmail, senderPassword, receiverEmail;

            Console.WriteLine("Enter File name:");
            fileName = Console.ReadLine();
            Console.WriteLine("Enter Sender email address:");
            senderEmail = Console.ReadLine();
            Console.WriteLine("Enter Sender password:");
            senderPassword = Console.ReadLine();
            Console.WriteLine("Enter Receiver email address:");
            receiverEmail = Console.ReadLine();

            string line;
            List<Country> Countries = new List<Country>();

            try
            {
                StreamReader file = new StreamReader(fileName);

                file.ReadLine();

                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(';');

                    if (!Countries.Any(c => c.CountryName == data[2]))
                    {
                        Country newCountry = new Country(data[2], data[0], data[1], Int32.Parse(data[4]));
                        Countries.Add(newCountry);
                    }
                    else
                    {
                        foreach (Country existingCountry in Countries)
                        {
                            if (existingCountry.CountryName == data[2])
                            {
                                existingCountry.AddScore(data[0], data[1], Int32.Parse(data[4]));
                            }
                        }
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("File not found.Please try again.");
                //Console.WriteLine(e.ToString());
            }
            

            var sortedCountries = Countries.OrderByDescending(x => x.AverageScore).ToList();

            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "ReportByCountry.csv");

            using (StreamWriter fs = File.CreateText(path))
            {
                fs.WriteLine("Country;Average score;Median score;Max score;Max score person;Min score;Min score person;Record count");
                foreach (Country c in sortedCountries)
                {
                    fs.WriteLine(c.GetCountryInCsv());
                }
            }

            try
            {            
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(receiverEmail);
                mail.Subject = "Attached file";
                mail.Body = "Mail with attachment";

                Attachment attachment;
                attachment = new Attachment(path);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential(senderEmail, senderPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Mail sent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorrect Sender email address/password or Recepient email address.Please try again.");
                //Console.WriteLine(e.ToString());
            }
        }
    }
}
