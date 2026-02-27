using MathApp.Data;
using MathApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathApp.Controllers
{
    public class MathController : Controller
    {
        private readonly MathAppContext _context;

        public MathController(MathAppContext context)
        {
            _context = context;
        }

        public IActionResult Calculate()
        {
            var token = HttpContext.Session.GetString("currentUser");

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(double Operand1, double Operand2, int Operation)
        {
            var token = HttpContext.Session.GetString("currentUser");

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            double result = 0;
            MathCalculations mathCalculation;

            try
            {
                mathCalculation = MathCalculations.Create(
                    Operand1,
                    Operand2,
                    Operation,
                    result,
                    token
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Operand2", ex.Message);
                return View();
            }

            switch (Operation)
            {
                case 1:
                    mathCalculation.Result = Operand1 + Operand2;
                    break;
                case 2:
                    mathCalculation.Result = Operand1 - Operand2;
                    break;
                case 3:
                    mathCalculation.Result = Operand1 * Operand2;
                    break;
                case 4:
                    mathCalculation.Result = Operand1 / Operand2;
                    break;
            }

            if (ModelState.IsValid)
            {
                _context.Add(mathCalculation);
                await _context.SaveChangesAsync();
            }

            ViewBag.Result = mathCalculation.Result;
            return View(mathCalculation);
        }

        public async Task<IActionResult> History()
        {
            var token = HttpContext.Session.GetString("currentUser");

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var history = await _context
                .MathCalculations.Where(m => m.FirebaseUuid == token)
                .OrderByDescending(m => m.CalculationID)
                .ToListAsync();

            return View(history);
        }

        public async Task<IActionResult> Clear()
        {
            var token = HttpContext.Session.GetString("currentUser");

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userHistory = _context.MathCalculations.Where(m => m.FirebaseUuid == token);
            _context.MathCalculations.RemoveRange(userHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction("History");
        }
    }
}
