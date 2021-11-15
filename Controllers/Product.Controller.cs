using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("products")]
public class ProductController : ControllerBase
{
  [HttpGet]
  [Route("")]
  public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
  {
    try
    {
      var products = await context.
      Products
       .Include(x => x.Category)
       .AsNoTracking()
       .ToListAsync();

      return Ok(products);
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi possivel buscar os produtos" });

    }

  }
  [HttpGet]
  [Route("{id:int}")]
  public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
  {
    try
    {
      var Product = await context.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
      return Ok(Product);
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi possivel buscar a categorias" });

    }
  }
  [HttpGet]
  [Route("categories/{id:int}")]
  public async Task<ActionResult<Product>> GetByCategory(int id, [FromServices] DataContext context)
  {
    try
    {
      var product = await context
        .Products
        .Include(x => x.Category)
        .AsNoTracking()
        .Where(x => x.CategoryId == id)
        .ToListAsync();

      return Ok(product.Select(x=>x.Category));
    }
    catch (Exception)
    {
      return BadRequest(new { message = "Não foi possivel buscar a categorias" });

    }
  }
  [HttpPost]
  [Route("")]
  public async Task<ActionResult<List<Product>>> Post([FromBody] Product model, [FromServices] DataContext context)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    try
    {
      context.Products.Add(model);
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
  public async Task<ActionResult<List<Product>>> Put(int id, [FromBody] Product model, [FromServices] DataContext context)
  {
    if (id != model.Id)
      return NotFound(new { message = "Categoria não encontrada" });

    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    try
    {
      context.Entry<Product>(model).State = EntityState.Modified;
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
  public async Task<ActionResult<List<Product>>> Delete(int id, [FromServices] DataContext context)
  {
    var Product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
    if (Product == null)
      return NotFound(new { message = "Não foi possivel encontrar a categoria" });

    try
    {
      context.Products.Remove(Product);
      await context.SaveChangesAsync();
      return Ok(new { message = "Categoria removida com sucesso" });
    }
    catch (Exception)
    {
      return NotFound(new { message = "Não foi possivel excluir a categoria" });

    }
  }
}