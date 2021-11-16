using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("users")]
  public class UserController : Controller
  {
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<User>> Post([FromBody] User model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Users.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);

      }
      catch
      {
        return BadRequest(new { message = "Não foi possivel cadastrar usuário" });
      }


    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model, [FromServices] DataContext context)
    {

      var user = await context.Users
                .AsNoTracking()
                .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                .FirstOrDefaultAsync();

      if (user == null)
        return NotFound(new { message = "usuário ou senha inválidos" });

      var token = TokenService.GenerateToken(user);

      return new
      {
        user = user,
        token = token
      };




    }


  }
}