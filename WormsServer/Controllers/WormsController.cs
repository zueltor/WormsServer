﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [Route("/")]
        [HttpGet]
        public IActionResult GetAction()
        {
            return new JsonResult(new { Worms_Version="1.18"});
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