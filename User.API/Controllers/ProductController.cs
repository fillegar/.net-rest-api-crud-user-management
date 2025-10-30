using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using User.Core;

namespace User.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Roles = Role.Admin)]
        [HttpGet("admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetAdminProducts()
        {
            return Ok(CreateProducts("admin"));
        }

        [Authorize(Roles = Role.SuperAdmin)]
        [HttpGet("superAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetSuperAdminProducts()
        {
            return Ok(CreateProducts("super admin"));
        }

        [Authorize(Roles = Role.BasicUser)]
        [HttpGet("basicUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetBasicUserProducts()
        {
            return Ok(CreateProducts("basic user"));
        }

        private static IEnumerable<Product> CreateProducts(string prefix)
        {
            return new List<Product>
            {
                new() { Name = $"{prefix} product1" },
                new() { Name = $"{prefix} product2" }
            };
        }
    }
}
