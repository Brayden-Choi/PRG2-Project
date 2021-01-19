//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_T06_Team9
{
    class Visitor : Person
    {
        public string PassportNo { get; set; }
        public string Nationality { get; set; }

        public Visitor(string name, string passportNo, string nationality) : base(name)
        {
            PassportNo = passportNo;
            Nationality = nationality;
        }

        /*public override double CalculateSHNCharges()
        {

        }*/

        public override string ToString()
        {
            return PassportNo + Nationality;
        }
    }
}
