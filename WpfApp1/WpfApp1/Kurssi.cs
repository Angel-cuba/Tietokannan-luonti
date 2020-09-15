using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Kurssi
    {
        public int Id { get; set; }
        public string Kurssinnimi { get; set; }
        public DateTime Alkamispvm { get; set; }
        public DateTime Paattymispvm { get; set; }
    }
}
