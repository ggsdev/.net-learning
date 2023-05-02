using Blog.Data;
using Blog.Models;
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
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
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
        public async Task<IActionResult> PostAsync([FromBody] Category body, [FromServices] BlogDataContext context)
        {
            try
            {
                await context.Categories.AddAsync(body);

                await context.SaveChangesAsync();
                return Created($"v1/categories/{body.Id}", body);

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
        public async Task<IActionResult> PutAsync([FromBody] Category body, [FromRoute] int id, [FromServices] BlogDataContext context)
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
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new { message = "Não foi possível atualizar a categoria" });
            }
            catch (System.Exception e)
            {

                return StatusCode(500, new { message = "Falha interna no servidor" });

            }


            return Ok(body);
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