using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webproject_M_R.Models
{
    public class Player
    {
        public string Firstname { get; set; }

        public String Lastname { get; set; }

        public DateTime Birthdate { get; set; }

        public double Height { get; set; }

        public DateTime Draft { get; set; }

        public double AveragePoints { get; set; }

        public Player() : this("", "", DateTime.MinValue, 0, DateTime.MinValue, 0) { }


        public Player(string firstname, string lastname, DateTime birthdate, double height, DateTime draft, double averagepoints)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Birthdate = birthdate;
            this.Height = height;
            this.Draft = draft;
            this.AveragePoints = averagepoints;
        }
    }
}