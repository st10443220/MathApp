using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MathApp.Models
{
    public class MathCalculations
    {
        [Key]
        public int CalculationID { get; set; }
        public int Operation { get; set; }

        [DisplayName("First Number")]
        public double Operand1 { get; set; }

        [DisplayName("Second Number")]
        public double Operand2 { get; set; }
        public double Result { get; set; }
    }
}
