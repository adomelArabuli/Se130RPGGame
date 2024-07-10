using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Se130RPGGame.Data.TestingModels;

namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("Deposit")]
        public IActionResult Deposit(decimal amount)
        {
            BankAccount bankAccount = new BankAccount();
            bankAccount.Deposit(amount);
            return Ok(bankAccount.Balance);
        }

        [HttpGet("Withdraw")]
        public IActionResult Withdraw(decimal amount)
        {
            BankAccount bankAccount = new BankAccount();
            bankAccount.Deposit(100);
            bankAccount.WithDraw(amount);
            return Ok(bankAccount.Balance);
        }
    }
}
