using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMedic.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Color { get; set; }
        public bool? AllDay { get; set; }
    }
}