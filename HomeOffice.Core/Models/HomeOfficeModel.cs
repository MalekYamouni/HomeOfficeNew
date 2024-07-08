using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeOffice.Core
{
    public class HomeOfficeTimeModel
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public DateTime Date { get; set; }
        public int TotalMinutes { get; set; }
    }
}