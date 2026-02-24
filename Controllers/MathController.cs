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

        // GET: Math/Calculate
        public IActionResult Calculate()
        {
            return View();
        }

        // POST: Math/Calculate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(MathCalculations model)
        {
            if (ModelState.IsValid)
            {
                switch (model.Operation)
                {
                    case 1:
                        model.Result = model.Operand1 + model.Operand2;
                        break;
                    case 2:
                        model.Result = model.Operand1 - model.Operand2;
                        break;
                    case 3:
                        model.Result = model.Operand1 * model.Operand2;
                        break;
                    case 4:
                        if (model.Operand2 == 0)
                        {
                            ModelState.AddModelError("Operand2", "Cannot divide by zero, bro.");
                            return View(model);
                        }
                        model.Result = model.Operand1 / model.Operand2;
                        break;
                    default:
                        ModelState.AddModelError(
                            "Operation",
                            "How did you manage to choose the wrong operation!"
                        );
                        return View(model);
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                ViewBag.Result = model.Result;

                return View(model);
            }

            return View(model);
        }

        // GET: Math/History
        public async Task<IActionResult> History()
        {
            var history = await _context
                .MathCalculations.OrderByDescending(m => m.CalculationID)
                .ToListAsync();
            return View(history);
        }
    }
}
