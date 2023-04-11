using FirstAPI.Data.DAL;
using FirstAPI.Dtos;
using FirstAPI.Dtos.CategoryDtos;
using FirstAPI.Dtos.ProductDtos;
using FirstAPI.Extentions;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(WebAppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAll(int page, int take, string? search)
        {
            var query = _context.Products.Where(p => !p.IsDelete);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            List<Product> products = query.ToList();
            ListDto<ProductListItemDto> productListDto = new();
            productListDto.TotalCount = query.Count();
            productListDto.List = products.Skip((page - 1) * take)
                .Select(p => new ProductListItemDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
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
            productReturnDto.ImageUrl = product.ImageUrl;
            productReturnDto.CreateDate = product.CreateDate;
            productReturnDto.EditDate = product.EditDate;
            return StatusCode(StatusCodes.Status200OK, productReturnDto);
        }

        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null) return NotFound();

            if (productCreateDto.Photo.CheckSize(2)) return BadRequest("Size bigger than 2mbgt");
            if (!productCreateDto.Photo.CheckType()) return BadRequest("Not image !");

            Product product = new();
            product.Name = productCreateDto.Name;
            product.Price = productCreateDto.Price;
            product.SalePrice = productCreateDto.SalePrice;
            product.ImageUrl = productCreateDto.Photo.SaveImage("img", _webHostEnvironment);
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
            if (existProd.ImageUrl != null)
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", existProd.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }                          
            }
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
            if(productUpdateDto.Photo!= null)
            {
                if (productUpdateDto.Photo.CheckSize(2)) return BadRequest("Size bigger than 2mbgt");
                if (!productUpdateDto.Photo.CheckType()) return BadRequest("Not image !");

                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", existProd.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                existProd.ImageUrl = productUpdateDto.Photo.SaveImage("img", _webHostEnvironment);
            }


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
