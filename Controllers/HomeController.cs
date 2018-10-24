using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DojoDachi.Models;
using Microsoft.AspNetCore.Http; // add this line to enable session


namespace DojoDachi.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            // get dojodachi 
            Pet dojodachi = SessionExtensions.GetObjectFromJson<Pet>(HttpContext.Session, "dojodachi");
            // the helper method will return null if it can't find the key "dojodachi"
            if (dojodachi == null) 
            {
                // create a dojodachi with default values and store it in session as a json object
                dojodachi = new Pet(20, 20, 50, 3);
                dojodachi.Image = "home.gif";
                dojodachi.Message = "Hola~ I am your dojodachi Pusheen. Please take good care of me meow~";
                SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            } 
            else 
            {
                string image = HttpContext.Session.GetString("image");
                // winning condition
                if (dojodachi.Happiness >= 100 || dojodachi.Fullness >= 100)
                {
                    dojodachi.Image = "win.gif";
                    dojodachi.Message = "You won!!!";
                }
                // losing condition
                if (dojodachi.Happiness <= 0 || dojodachi.Fullness <= 0 || dojodachi.Energy <= 0)
                {
                    dojodachi.Image = "lose.gif";
                    dojodachi.Message = "Oh no... RIP dojodachi :(";
                }
                SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            }
            return View("Index", dojodachi);
        }

        // restart button
        [HttpPost("")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // feed method
        [HttpPost("feed")]
        public IActionResult Feed()
        {
            Pet dojodachi = SessionExtensions.GetObjectFromJson<Pet>(HttpContext.Session, "dojodachi");
            string image = HttpContext.Session.GetString("image");
            // 25% chance of refusal
            if (dojodachi.Meals > 0)
            {
                dojodachi.Meals -= 1;
                if (this.Refuse()) 
                {
                    dojodachi.Image = "refuse.gif";
                    dojodachi.Message = $"Uh oh refused! Meal -1, Fullness +0";
                } 
                else 
                {
                    dojodachi.Image = "eat.gif";
                    Random rand = new Random();
                    int points = rand.Next(5, 11);
                    dojodachi.Fullness += points;
                    dojodachi.Message = $"Yum yum~ Meal -1, Fullness +{points}";
                }
            }
            else 
            {
                dojodachi.Image = "hungry.gif";
                dojodachi.Message = "Uh oh, we ran out of food. Go get some work done first, dojodachi~";
            }
            SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            return RedirectToAction("Index", dojodachi);
        }
        
        // play method
        [HttpPost("play")]
        public IActionResult Play()
        {
            Pet dojodachi = SessionExtensions.GetObjectFromJson<Pet>(HttpContext.Session, "dojodachi");
            string image = HttpContext.Session.GetString("image");
            dojodachi.Image = "play.gif";
            dojodachi.Energy -= 5;
            // 25% chance of refusal
            if (this.Refuse()) {
                dojodachi.Image = "refuse.gif";
                dojodachi.Message = $"Uh oh refused! Energy -1, Fullness +0";
            } else {
                dojodachi.Image = "play.gif";
                Random rand = new Random();
                int points = rand.Next(5, 11);
                dojodachi.Happiness += points;
                dojodachi.Message = $"Your dojodachi had fun! Energy -5, Happiness +{points}";
            }
            SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            return RedirectToAction("Index", dojodachi);
        }

        // work method
        [HttpPost("work")]
        public IActionResult Work()
        {
            Pet dojodachi = SessionExtensions.GetObjectFromJson<Pet>(HttpContext.Session, "dojodachi");
            string image = HttpContext.Session.GetString("image");
            dojodachi.Image = "work.gif";
            Random rand = new Random();
            int points = rand.Next(1, 4);
            dojodachi.Meals += points;
            dojodachi.Energy -= 5;
            dojodachi.Message = $"Dojodachi did some work! Energy -5, Meal +{points}";
            SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            return RedirectToAction("Index", dojodachi);
        }

        // sleep method
        [HttpPost("sleep")]
        public IActionResult Sleep()
        {
            Pet dojodachi = SessionExtensions.GetObjectFromJson<Pet>(HttpContext.Session, "dojodachi");
            string image = HttpContext.Session.GetString("image");
            dojodachi.Image = "sleep.gif";
            dojodachi.Energy += 15;
            dojodachi.Happiness -= 5;
            dojodachi.Fullness -= 5;
            dojodachi.Message = "Meow gut night~~ Energy +15, Happiness -5, Fullness -5";
            SessionExtensions.SetObjectAsJson(HttpContext.Session, "dojodachi", dojodachi);
            return RedirectToAction("Index", dojodachi);
        }

        public bool Refuse()
        {
            Random rand = new Random();
            int number = rand.Next(1, 5);
            // 25% chance
            return number == 1;
        }
    }
}
