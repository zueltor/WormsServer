using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WorldOfWorms.Behaviors;
using WormsServer.Behaviors;
using WormsServer.Models;

namespace WormsServer.Controllers
{
    public class WormsController : Controller
    {
        private readonly IBehavior wormBehavior;

        public WormsController(IBehavior wormBehavior)
        {
            this.wormBehavior = wormBehavior;
        }

        [Route("/get")]
        [HttpGet]
        public IActionResult GetAction()
        {
            //var beh = wormBehavior as BehaviorSmart;
           
            //List<Dictionary<string, string>> kekw = new();
            // foreach (var triplet in BehaviorSmart.wormFoodDictionary2)
            // {
            //     Dictionary<string, string> kek = new();
            //     foreach (var pair in triplet.Value)
            //     {
            //         kek[$"{pair.Key.Name}-{pair.Key.Lifeforce}-{pair.Key.WormPosition}"] = pair.Value.ToString();
            //     }
            //     kekw.Add(kek);
            //     
            // }
            return new JsonResult(new { Version="1.14"});
            //return new JsonResult("V 1.6");
        }

        [Route("/{name}/getAction/{step?}/{run?}")]
        [HttpPost]
        public IActionResult GetAction(string name, [FromBody] WorldModel world, int step = 0, int run = 0)
        {
            var currentWorm = world.Worms.FirstOrDefault(worm => worm.Name.Equals(name));
            if (currentWorm is null)
            {
                return BadRequest("No such worm");
            }

            var action = IBehavior.ParseResponse(wormBehavior.DoSomething(world, currentWorm, step, run));
            return new JsonResult(new
                WormAction { Direction = action.Direction, Split = action.Split });
        }
    }
}