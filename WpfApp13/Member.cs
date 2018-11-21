using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConsoleApp1;

namespace WpfApp13
{
    public class Member 
    {

        [Key]
        public int MemberID { get; set; }

        public Member()
        {

        }
    }
}
