using FirstAPI.Data.DAL;
using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly WebAppDbContext _context;

        public ProductController(WebAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Products.ToList());
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne(int? id)
        {
            if (id == null || id == 0) return StatusCode(StatusCodes.Status404NotFound);
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return StatusCode(StatusCodes.Status404NotFound);
            return StatusCode(StatusCodes.Status200OK, product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product? product)
        {
            if (product == null) return BadRequest();
            _context.Products.Add(product);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var existProd = _context.Products.FirstOrDefault(p=>p.Id == id);
            if (existProd == null) return NotFound();
            _context.Products.Remove(existProd);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int? id ,Product product)
        {
            if(product == null || id == null || id == 0) return BadRequest();
            var existProd = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existProd == null) return NotFound();
            existProd.Name = product.Name;
            existProd.Price = product.Price;
            existProd.IsActive = product.IsActive;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch]
        public IActionResult ChangeStatus(int? id, bool status)
        {
            if(id == null|| id == 0) return BadRequest();
            var existProd = _context.Products.FirstOrDefault(p=>p.Id == id);
            if (existProd == null) return NotFound();
            existProd.IsActive = status;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
