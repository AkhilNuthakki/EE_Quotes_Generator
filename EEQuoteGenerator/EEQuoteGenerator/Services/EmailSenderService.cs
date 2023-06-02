using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using EEQuoteGenerator.Models;


namespace EEQuoteGenerator.Services
{
    public class EmailSenderService
    {
        private const string MAIL_HOST = "smtp.office365.com";
        //private const string EMAIL_USERNAME = "noreply.ee.quote.tool@gmail.com";
        private const string EMAIL_USERNAME = "No-reply.ee-quote-tool@outlook.com";
        private const string EMAIL_PASSWORD = "UOLee@2022";

        public static async Task<bool> SendEmailAsync(string email, string subject, List<string> Attachments, Quote Quote, User User)
        {
            try
            {
                // Defining the Client Object
                SmtpClient client = new SmtpClient(MAIL_HOST, 587);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(EMAIL_USERNAME, EMAIL_PASSWORD);
                client.EnableSsl = true;
                client.Credentials = credentials;

                // Defining the Email Object
                MailMessage Email = new MailMessage(EMAIL_USERNAME, email);
                Email.Subject = subject;
                Email.IsBodyHtml = true;
                // Creating a message in HTML using AlternateView
                AlternateView HTMLView = CreateEmailBody(Quote, User);
                Email.AlternateViews.Add(HTMLView);
                foreach (string attachment in Attachments){
                    Email.Attachments.Add(new Attachment(attachment));
                }
                //clent send the email.
                await client.SendMailAsync(Email);

                //disposing the attachement from the email object
                Email.Attachments.Dispose();
                //disposing the email
                Email.Dispose();

                return true;
            }
            catch 
            {
                return false;
            }
          
        }

        public static AlternateView CreateEmailBody(Quote Quote, User User)
        {

            // Defining a Email Body using a string
            string EmailBody = "<p style='font-size:10pt;font-family:Century Gothic;'>Dear X_CUSTOMER_FIRSTNAME_X,</p>";
            EmailBody += "<br> <p style='font-size:10pt;font-family:Century Gothic;'>Many Thanks for your enquiry into the potential install of solar PV  at X_SITE_ADDRESS_X</p>";
            EmailBody += "<p style='font-size:10pt;font-family:Century Gothic;'>please find below our quotation/contract following the site visit.</p> <br>";
            EmailBody += "<p style='font-size:10pt;font-family:Century Gothic;'>Kind Regards,<br><span style='font-weight:bold;'>X_USER_NAME_X</span><br><span style='color:#96BB22;'>X_USER_ROLE_X</span></p>";
            EmailBody += "<img width='675' height='151' style='width:7.0333in;height:1.575in' src='cid:FooterImageID'/>";
            EmailBody += "<p style='font-size:10pt;font-family:Century Gothic;'><span style='font-weight:bold;color:#96BB22;'>Environmental Energies Limited</span><br>Harborough Innovation Centre,<br>Airfield Business Park, Leicester Rd,<br>Market Harborough LE16 7WB</p>";
            EmailBody += "<p style='font-size:10pt;font-family:Century Gothic;font-weight:bold;'>E: X_USER_EMAIL_X | W: <a href='https://www.environmentalenergies.co.uk/'>www.environmentalenergies.co.uk</a></p>";
            EmailBody += "<p style='font-size:10pt;font-family:Century Gothic;font-weight:bold;'>T: 01858 525 407 </p>";
            EmailBody += "<p style='font-size:7.5pt;line-height:105%;font-family:Century Gothic;color:#999999;'>This message (and any attachment(s)) is confidential and may contain privileged information. It is for the intended recipient only. Any dissemination, distribution, copying, disclosure or use of this message or its content is strictly prohibited unless authorised by us. If you receive this message in error, please notify the sender as soon as possible and destroy the original message, not keeping a copy.</p>";

            //Replacing the dynamic data in the email body
            EmailBody = EmailBody.Replace("X_CUSTOMER_FIRSTNAME_X", Quote.Customer.FirstName);
            string siteAddress = Quote.Customer.Address.ToUpper() + " " + Quote.Customer.PostCode.ToUpper();
            EmailBody = EmailBody.Replace("X_SITE_ADDRESS_X", siteAddress);
            EmailBody = EmailBody.Replace("X_USER_NAME_X", User.FirstName + " " + User.LastName);
            EmailBody = EmailBody.Replace("X_USER_ROLE_X", User.UserRole);
            EmailBody = EmailBody.Replace("X_USER_EMAIL_X ",User.UserEmail);

            //creating the html view using AlternateView and adding the footer image (signature Image)
            AlternateView AVHtmlView = AlternateView.CreateAlternateViewFromString(EmailBody, null, "text/html");
            string EmailFooterUrl = AppDomain.CurrentDomain.BaseDirectory + "Resources\\EmailFooter.png";
            LinkedResource FooterImage = new LinkedResource(EmailFooterUrl);
            FooterImage.ContentId = "FooterImageID";
            AVHtmlView.LinkedResources.Add(FooterImage);

            return AVHtmlView;
        }


    }
}
