using System.Runtime.InteropServices;
using System.Xml.Linq;
using System;

namespace JwtWebApi.tables
{
    public class Guests
    {
        public int Id { get; set; }
        public string name { get; set; } = string.Empty;
        public int room { get; set; }
    }
}
