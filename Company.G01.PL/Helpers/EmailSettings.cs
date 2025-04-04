using System.Net;
using System.Net.Mail;

namespace Company.G01.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email) 
        {
            try 
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("mohamed.elmoghaazi@gmail.com", "kplncoingqqjzlfc");
                client.Send("mohamed.elmoghaazi@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            #region Pass
            //kplncoingqqjzlfc
            //kpln coin gqqj zlfc 
            #endregion
        }
    }
}
