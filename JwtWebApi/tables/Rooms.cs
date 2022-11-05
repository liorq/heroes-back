using System.Runtime.InteropServices;
using System.Xml.Linq;
using System;

namespace JwtWebApi.tables
{
    public class Rooms
    {
        public int Id { get; set; }
        public int number { get; set; }
        public int capacity { get; set; }
    }
}
