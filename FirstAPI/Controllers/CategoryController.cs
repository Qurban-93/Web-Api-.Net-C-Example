using AutoMapper;
using FirstAPI.Data.DAL;
using FirstAPI.Dtos;
using FirstAPI.Dtos.CategoryDtos;
using FirstAPI.Extentions;
using FirstAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly WebAppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public CategoryController(WebAppDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _context = context;
            _environment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddCategory([FromForm]CategoryCreateDto categoryCreateDto)
        {
            if (_context.Categories.Any(c => c.CategoryName == categoryCreateDto.CategoryName))
            { return BadRequest("Eyni adli Category var"); }

            if (categoryCreateDto.Photo.CheckSize(2)) return BadRequest("Size bigger than 2mbgt");
            if (!categoryCreateDto.Photo.CheckType()) return BadRequest("Not image !");

        
            Category newCategory = new();
            newCategory.CreateDate = DateTime.Now;
            newCategory.CategoryName = categoryCreateDto.CategoryName;
            newCategory.CategoryDescription= categoryCreateDto.CategoryDescription;
            newCategory.ImageUrl = categoryCreateDto.Photo.SaveImage("img",_environment);
            newCategory.IsActive = true;
            newCategory.IsDeleted= false;
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetAllCategory(int page,string? search,int take)
        {
            ListDto<CategoryReturnDto> categoryListDto = new();
            var query = _context.Categories.Where(c => !c.IsDeleted);
            categoryListDto.TotalCount = query.Count();
            if (!string.IsNullOrWhiteSpace(search))
            {
               query = _context.Categories.Where(c => !c.IsDeleted && c.CategoryName.Contains(search));
            }
             
            
            categoryListDto.List = query.Skip((page-1)*take).Take(take).Select(c=>new CategoryReturnDto()
            {
                CategoryDescription= c.CategoryDescription,
                CategoryName= c.CategoryName,
                ImageUrl= "https://localhost:7232/img/"+c.ImageUrl,

            }).ToList();
            

            return Ok(categoryListDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int? id,CategoryUpdateDto categoryUpdateDto)
        {
            if(id == null || id == 0) return NotFound();
            var existCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(existCategory == null) return NotFound();
            bool checkName = _context.Categories.Any(c => c.CategoryName == categoryUpdateDto.CategoryName && c.Id != id);
            if (checkName) return BadRequest("eyni adli categori artiq var");
            if(categoryUpdateDto.Photo != null)
            {
                if (categoryUpdateDto.Photo.CheckSize(2)) return BadRequest("Size bigger than 2mbgt");
                if (!categoryUpdateDto.Photo.CheckType()) return BadRequest("Not image !");

                string fullPath = Path.Combine(_environment.WebRootPath, "img", existCategory.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                existCategory.ImageUrl = categoryUpdateDto.Photo.SaveImage("img",_environment);
            }

            existCategory.EditDate = DateTime.Now;
            existCategory.CategoryDescription= categoryUpdateDto.CategoryDescription;
            existCategory.CategoryName = categoryUpdateDto.CategoryName;
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpGet("{id}")]
        public IActionResult GetOne(int? id) 
        { 
            if(id ==null || id==0) return NotFound();

            var category = _context.Categories.Include(c=>c.Products)
                .FirstOrDefault(c=>c.Id== id);
            if(category == null) return NotFound();
            CategoryReturnDto categoryReturnDto = _mapper.Map<CategoryReturnDto>(category);

            //CategoryReturnDto categoryReturnDto = new();
            //categoryReturnDto.CategoryName = category.CategoryName;
            //categoryReturnDto.CategoryDescription= category.CategoryDescription;

            return Ok(categoryReturnDto);
        
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if(id == null || id==0) return NotFound();
            var category = _context.Categories.FirstOrDefault(c=>c.Id== id);
            if(category == null) return NotFound();

            if (category.ImageUrl != null)
            {
                string fullPath = Path.Combine(_environment.WebRootPath, "img", category.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

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
