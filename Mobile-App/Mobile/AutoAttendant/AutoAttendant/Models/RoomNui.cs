using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class RoomNui
    {
        List<String> _A;
        List<String> _B;
        List<String> _C;

        public RoomNui(List<string> a, List<string> b, List<string> c)
        {
            A = a;
            B = b;
            C = c;
        }

        public List<string> A { get => _A; set => _A = value; }
        public List<string> B { get => _B; set => _B = value; }
        public List<string> C { get => _C; set => _C = value; }
    }
}
