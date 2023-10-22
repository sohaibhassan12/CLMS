using Cloud_License_Managment_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Cloud_License_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly CLMS_DbContext _DbContext;

        public ProductController(IConfiguration configuration, CLMS_DbContext context)
        {
            _configuration = configuration;
            _DbContext = context;
        }

        [HttpPost]
        [Route("SaveProduct")]
        public IActionResult SaveProduct(Products model)
        {
            if(ModelState.IsValid)
            {
                _DbContext.Add(model);
                _DbContext.SaveChanges();
            }
            return Ok(model);
        }


    }
}
