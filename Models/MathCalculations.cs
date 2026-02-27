using System.ComponentModel.DataAnnotations;

namespace MathApp.Models
{
    public class MathCalculations
    {
        private MathCalculations() { }

        [Key]
        public int CalculationID { get; set; }

        [Display(Name = "First Number")]
        public double Operand1 { get; set; }

        [Display(Name = "Second Number")]
        public double Operand2 { get; set; }

        public int Operation { get; set; }

        public double Result { get; set; }

        public string? FirebaseUuid { get; set; }

        public static MathCalculations Create(
            double operand1,
            double operand2,
            int operation,
            double result,
            string? firebaseUuid
        )
        {
            if (operation == 4 && operand2 == 0)
            {
                throw new ArgumentException("Cannot divide by zero, bro.");
            }

            return new MathCalculations
            {
                Operand1 = operand1,
                Operand2 = operand2,
                Operation = operation,
                Result = result,
                FirebaseUuid = firebaseUuid,
            };
        }
    }
}
