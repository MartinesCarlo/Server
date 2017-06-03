using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Spieler
    {
        private int id;
        private String benutzername;
        private String kennwort;

        public Spieler(int i, string b, string k)
        {
            Id = i;
            Benutzername = b;
            Kennwort = k;
        }

        public string Kennwort { get => kennwort; set => kennwort = value; }
        public string Benutzername { get => benutzername; set => benutzername = value; }
        public int Id { get => id; set => id = value; }
    }
}
