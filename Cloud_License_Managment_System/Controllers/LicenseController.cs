using Cloud_License_Managment_System.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Cloud_License_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly CLMS_DbContext _DbContext;

        public LicenseController(IConfiguration configuration, CLMS_DbContext context)
        {
            _configuration = configuration;
            _DbContext = context;
        }
        [HttpPost]
        [Route("GenerateLicense/{id}")]
        public IActionResult GenerateLicense(long id)
        {

            string query = "SELECT p.Id,u.Email from Products p with(NOLOCK) JOIN Users u with(NOLOCK) ON p.UserId = u.Id WHERE p.Id = " + id + " u.Role = 'Admin'";

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _configuration.GetConnectionString("DBConnection");
            dynamic result;
            using (conn)
            {
                result = conn.Query<dynamic>(query).FirstOrDefault();
            }
            if (result != null)
            {
                Random random = new Random();
                int randomNumber = random.Next(1000, 5000);
                string updatequery = "Update License set Status ='Active', LisenceNumber = "+randomNumber+" where ProductId = " + result.Id + "";
                using (conn)
                {
                     conn.Query(updatequery);
                }
                SendMail(result.Email, randomNumber);
            }
            else
            {
                return BadRequest("No Product Found");
            }
            return Ok("License Generate sucessfully!");
        }
        [HttpPost]
        [Route("ActivateLicense/{LicenseKey}")]
        public IActionResult ActivateLicense(string LicenseKey)
        {
            string query = "SELECT li.Id from Licenses li with(NOLOCK)" +
                " JOIN Products p with(NOLOCK) ON li.ProductId = p.Id " +
                " JOIN Users u with(NOLOCK) ON p.UserId = u.Id" +
                " WHERE li.LisenceNumber = "+LicenseKey+"u.Role = 'User'";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _configuration.GetConnectionString("DBConnection");
            long result;
            using (conn)
            {
                result = conn.Query<long>(query).FirstOrDefault();
            }
            if(result > 0)
            {
                string updatequery = "Update License set Status ='Active' where ProductId = " + result + "";
                using (conn)
                {
                    conn.Query(updatequery);
                }

            }
            else
            {
                return BadRequest("No License Found");
            }
            return Ok("License Activated");
        }

        private void SendMail(string Email,int code)
        {
            string smtpServer = "your-smtp-server.com";
            int smtpPort = 587; // Use the appropriate port for your email provider
            string senderEmail = "your-email@example.com";
            string senderPassword = "your-password";
            string recipientEmail = Email;
            string subject = "Verification Code";
            string body = $"Your verification code is: {code}";
            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true; // Use SSL if required by your email provider

                // Create and send the email
                using (MailMessage message = new MailMessage(senderEmail, recipientEmail, subject, body))
                {
                    client.Send(message);
                }
            }
        }
    }
}
