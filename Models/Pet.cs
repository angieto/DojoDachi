using System;
using System.ComponentModel.DataAnnotations;

namespace DojoDachi.Models
{
    public class Pet
    {
        public int Happiness {get; set;}
        public int Fullness {get; set;}
        public int Energy {get; set;}
        public int Meals {get; set;}
        public string Message {get; set;}
        public string Image {get; set;}
        // create a class constructor
        public Pet(int happiness, int fullness, int energy, int meals)
        {
            Happiness = happiness;
            Fullness = fullness;
            Energy = energy;
            Meals = meals;
            Message = "";
            Image = "";
        }
    }
}