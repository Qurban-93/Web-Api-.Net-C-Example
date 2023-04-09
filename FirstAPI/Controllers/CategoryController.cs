using FirstAPI.Data.DAL;
using FirstAPI.Dtos.CategoryDtos;
using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly WebAppDbContext _context;

        public CategoryController(WebAppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryCreateDto categoryCreateDto)
        {
            Category newCategory = new();
            newCategory.CreateDate = DateTime.Now;
            newCategory.CategoryName = categoryCreateDto.CategoryName;
            newCategory.CategoryDescription= categoryCreateDto.CategoryDescription;
            newCategory.IsActive = true;
            newCategory.IsDeleted= false;
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var query = _context.Categories.Where(c => !c.IsDeleted);
            CategoryListDto categoryListDto = new();
            categoryListDto.Items = query.Select(c=>new CategoryReturnDto()
            {
                CategoryDescription= c.CategoryDescription,
                CategoryName= c.CategoryName,

            }).ToList();
            categoryListDto.TotalCount = query.Count();

            return Ok(categoryListDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int? id,CategoryUpdateDto categoryUpdateDto)
        {
            if(id == null || id == 0) return NotFound();
            var existCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(existCategory == null) return NotFound();
            existCategory.EditDate = DateTime.Now;
            existCategory.CategoryDescription= categoryUpdateDto.CategoryDescription;
            existCategory.CategoryName= categoryUpdateDto.CategoryName;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpGet("{id}")]
        public IActionResult GetOne(int? id) 
        { 
            if(id ==null || id==0) return NotFound();

            var category = _context.Categories.FirstOrDefault(c=>c.Id== id);
            if(category == null) return NotFound();
            CategoryReturnDto categoryReturnDto = new();
            categoryReturnDto.CategoryName = category.CategoryName;
            categoryReturnDto.CategoryDescription= category.CategoryDescription;

            return Ok(categoryReturnDto);
        
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if(id == null || id==0) return NotFound();
            var category = _context.Categories.FirstOrDefault(c=>c.Id== id);
            if(category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch]
        public IActionResult UpdateStatus(int? id,bool status)
        {
            if(id ==null || id==0) return NotFound();
            var category = _context.Categories.FirstOrDefault(c=>c.Id== id);
            if(category == null) return NotFound();
            category.IsDeleted = status;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
