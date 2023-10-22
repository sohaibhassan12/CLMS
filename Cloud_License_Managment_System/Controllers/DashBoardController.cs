using Cloud_License_Managment_System.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cloud_License_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CLMS_DbContext _DbContext;

        public DashBoardController(IConfiguration configuration, CLMS_DbContext context)
        {
            _configuration = configuration;
            _DbContext = context;
        }
        [HttpGet]
        [Route("GetDashBoard/{UserID}")]
        public IActionResult GetDashBoard(long UserID)
        {
            string checkUser = "select role from users where id = " + UserID + "";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _configuration.GetConnectionString("DBConnection");
            string? result;
            using (conn)
            {
                result = conn.Query<string>(checkUser).FirstOrDefault();
            }
            if(!string.IsNullOrEmpty(result))
            {
                if(result.ToUpper() == "ADMIN")
                {
                    string query = "SELECT li.LisenceNumber,li.ProductId,li.Status  from Licenses li with(NOLOCK)" +
                        " JOIN Products p with(NOLOCK) ON li.ProductId = p.Id " +
                        " JOIN Users u with(NOLOCK) ON p.UserId = u.Id"+
                        " WHERE li.Status = 'Active'";
                    Licenses? licenses = new Licenses();
                    using (conn)
                    {
                        licenses = conn.Query<Licenses>(query).FirstOrDefault();
                    }
                    return Ok(licenses);

                }
                if(result.ToUpper() == "USER")
                {
                    string query = "SELECT li.LisenceNumber,li.ProductId,li.Status  from Licenses li with(NOLOCK)" +
                        " JOIN Products p with(NOLOCK) ON li.ProductId = p.Id " +
                        " JOIN Users u with(NOLOCK) ON p.UserId = u.Id" +
                        " WHERE li.Status = 'Active'  AND u.Id = "+ UserID + "";
                    Licenses? licenses = new Licenses();
                    using (conn)
                    {
                        licenses = conn.Query<Licenses>(query).FirstOrDefault();
                    }
                    return Ok(licenses);
                }
            }
            return Ok();
        }



    }
}
