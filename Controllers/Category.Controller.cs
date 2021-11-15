using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("categories")]
public class CategoryController : ControllerBase
{
  [HttpGet]
  [Route("")]
  public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
  {
    try
    {
      var categories = await context.Categories.AsNoTracking().ToListAsync();
      return Ok(categories);
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi possivel buscar as categorias" });

    }

  }
  [HttpGet]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
  {
    try
    {
      var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      return Ok(category);
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi possivel buscar a categorias" });

    }
  }

  [HttpPost]
  [Route("")]
  public async Task<ActionResult<List<Category>>> Post([FromBody] Category model, [FromServices] DataContext context)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    try
    {
      context.Categories.Add(model);
      await context.SaveChangesAsync();
      return Ok(model);
    }
    catch
    {
      return BadRequest(new { message = "Não foi possivel criar uma categoria" });
    }


  }
  [HttpPut]
  [Route("{id:int}")]
  public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
  {
    if (id != model.Id)
      return NotFound(new { message = "Categoria não encontrada" });

    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    try
    {
      context.Entry<Category>(model).State = EntityState.Modified;
      await context.SaveChangesAsync();
      return Ok(model);

    }
    catch (DbUpdateConcurrencyException)
    {
      return BadRequest(new { message = "Não foi possivel atualizar a categoria" });

    }

  }
  [HttpDelete]
  [Route("{id:int}")]
  public async Task<ActionResult<List<Category>>> Delete(int id, [FromServices] DataContext context)
  {
    var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    if (category == null)
      return NotFound(new { message = "Não foi possivel encontrar a categoria" });

    try
    {
      context.Categories.Remove(category);
      await context.SaveChangesAsync();
      return Ok(new { message = "Categoria removida com sucesso" });
    }
    catch (Exception)
    {
      return NotFound(new { message = "Não foi possivel excluir a categoria" });

    }
  }
}