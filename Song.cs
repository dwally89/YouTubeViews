using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouTube_Views_CSharp
{
    public class Song
    {
        public string Name { get; set; }        
        public int GetTotalViews()
        {
            return Views + HiddenNumbers;
        }
        public int Views {get;set;}        
        public int ViewsPerDay { get; set; }
        public int HiddenNumbers { get; set; }
        public DateTime DateAdded { get; set; }
        public string getStatus()
        {
            if (ViewsPerDay > Views)
            {
                return "Increasing";
            }
            else if (ViewsPerDay == Views)
            {
                return "Stable";
            }
            else
            {
                return "Decreasing";
            }
        }

        public Song() { }
        public Song(string name, int views, DateTime dateAdded, int hiddenNumbers)
        {
            Name = name;
            Views = views;
            DateAdded = dateAdded;
            HiddenNumbers = hiddenNumbers;
        }
        public override string ToString()
        {
            return Name + " " + Views + " " + DateAdded.ToShortDateString() + " " + HiddenNumbers;
        }
    }
}
