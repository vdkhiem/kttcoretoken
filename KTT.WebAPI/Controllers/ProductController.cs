using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KTT.WebAPI.Models;
using KTT.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KTT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    
    public class ProductController : BaseController<Product>
    {
        public ProductController(IEntityMaint<Product> entityMaint) : base(entityMaint)
        {
            if (this.entityMaint.GetAll().Any()) return;
            this.entityMaint.Add(new Product() { Id = 1, Code = "iphone1", Description = "iPhone 1" });
            this.entityMaint.Add(new Product() { Id = 2, Code = "iphone2", Description = "iPhone 2" });
            this.entityMaint.Add(new Product() { Id = 3, Code = "iphone3", Description = "iPhone 3" });
            this.entityMaint.Add(new Product() { Id = 4, Code = "iphone4", Description = "iPhone 4" });
        }

        [HttpGet("[action]/{code}")]
        public IActionResult GetByCode(string code)
        {
            var product = entityMaint.Search(p => p.Code == code).FirstOrDefault();

            if (product != null)
                return Ok(product);
            else
                return NotFound();
        }
    }
}