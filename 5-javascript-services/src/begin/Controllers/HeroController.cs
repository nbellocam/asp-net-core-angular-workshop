using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TourOfHeroes.Models;

namespace TourOfHeroes.Controllers
{
  [Route("api/[controller]")]
  public class HeroController : Controller
  {
    private static List<Hero> heroes = new List<Hero> {
      new Hero() { Id= 11, Name= "Mr. Nice"},
      new Hero() { Id= 12, Name= "Narco"},
      new Hero() { Id= 13, Name= "Bombasto"},
      new Hero() { Id= 14, Name= "Celeritas"},
      new Hero() { Id= 15, Name= "Magneta"},
      new Hero() { Id= 16, Name= "RubberMan"},
      new Hero() { Id= 17, Name= "Dynama"},
      new Hero() { Id= 18, Name= "Dr IQ"},
      new Hero() { Id= 19, Name= "Magma"}
    };

    private static int nextId = 21;

    [HttpGet]
    public IEnumerable<Hero> Get()
    {
      return heroes;
    }

    [HttpGet("{id}")]
    public Hero Get(int id)
    {
      return heroes.FirstOrDefault(h => h.Id == id);
    }

    [HttpPost]
    public Hero Post([FromBody]Hero hero)
    {
      hero.Id = nextId++;
      heroes.Add(hero);
      return hero;
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody]Hero hero)
    {
      var dbHero = heroes.FirstOrDefault(h => h.Id == hero.Id);
      dbHero.Name = hero.Name;
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var dbHero = heroes.FirstOrDefault(h => h.Id == id);
      heroes.Remove(dbHero);
    }
  }
}