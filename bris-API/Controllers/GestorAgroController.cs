using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/ga")]
    [ApiController]
    public class GestorAgroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GestorAgroController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
        }
    }
}