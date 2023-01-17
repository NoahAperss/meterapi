using System;
using System.Linq;
using System.Security.Cryptography;
using meterapi.Data;
using meterapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace meterapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }


    }
}