using FirstAPI.Data.DAL;
using FirstAPI.Dtos.ProductDtos;
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
        public IActionResult GetAll(int page,int take)
        {
            var Query = _context.Products.Where(p => !p.IsDelete);

            List<Product> products = Query.ToList();
            ProductListDto productListDto = new ProductListDto();
            productListDto.TotalCount = Query.Count();
            productListDto.Items = products.Skip((page-1)*take)
                .Select(p => new ProductListItemDto
            {
                Name = p.Name,
                Price = p.Price,
                SalePrice = p.SalePrice,
                CreateDate = p.CreateDate,
                EditDate = p.EditDate,

            }).ToList();
            return Ok(productListDto);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne(int? id)
        {
            if (id == null || id == 0) return StatusCode(StatusCodes.Status404NotFound);
            var product = _context.Products
                .Where(p => !p.IsDelete)
                .FirstOrDefault(p => p.Id == id);
            if (product == null) return StatusCode(StatusCodes.Status404NotFound);
            ProductReturnDto productReturnDto = new ProductReturnDto();
            productReturnDto.Name = product.Name;
            productReturnDto.Price = product.Price;
            productReturnDto.SalePrice = product.SalePrice;
            productReturnDto.CreateDate = product.CreateDate;
            productReturnDto.EditDate = product.EditDate;
            return StatusCode(StatusCodes.Status200OK, productReturnDto);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null) return NotFound();
            Product product = new();
            product.Name = productCreateDto.Name;
            product.Price = productCreateDto.Price;
            product.SalePrice = productCreateDto.SalePrice;
            product.IsActive = true;
            product.IsDelete = productCreateDto.IsDeleted;
            product.CreateDate = DateTime.Now;


            _context.Products.Add(product);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var existProd = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existProd == null) return NotFound();
            _context.Products.Remove(existProd);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int? id, ProductUpdateDto productUpdateDto)
        {
            if (productUpdateDto == null || id == null || id == 0) return BadRequest();
            var existProd = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existProd == null) return NotFound();
            existProd.Name = productUpdateDto.Name;
            existProd.Price = productUpdateDto.Price;
            existProd.SalePrice = productUpdateDto.SalePrice;
            existProd.IsDelete = productUpdateDto.isDelete;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch]
        public IActionResult ChangeStatus(int? id, bool status)
        {
            if (id == null || id == 0) return BadRequest();
            var existProd = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existProd == null) return NotFound();
            existProd.IsActive = status;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
