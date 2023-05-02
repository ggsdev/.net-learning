using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna do servidor"));
            }

        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync((category) => category.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel body, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var category = new Category
                {
                    Name = body.Name,
                    Slug = body.Slug
                };
                await context.Categories.AddAsync(category);

                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", category);

            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { message = "Não foi possível incluir a categoria" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }



        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromBody] EditorCategoryViewModel body, [FromRoute] int id, [FromServices] BlogDataContext context)
        {

            var category = await context.Categories.FirstOrDefaultAsync((category) => category.Id == id);

            if (category == null)
                return NotFound();

            try
            {
                category.Name = body.Name;
                category.Slug = body.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();
                return Ok(body);

            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new { message = "Não foi possível atualizar a categoria" });
            }
            catch (System.Exception e)
            {

                return StatusCode(500, new { message = "Falha interna no servidor" });

            }


        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync((category) => category.Id == id);

            if (category == null)
                return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}