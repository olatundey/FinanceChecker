using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//Please note, below code can only run and be used on a ASP .NET Core WEB API Project
namespace WebAPI_FinanceChecker_bal_trans.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/BankA")]
    [ApiController]
    public class FinanceApiController : ControllerBase
    {
        private readonly ILogger<FinanceApiController> _logger;

        public FinanceApiController(ILogger<FinanceApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("balance/{accountNumber}")]
        public async Task<ActionResult<decimal>> GetBalance(int accountNumber)
        {
            var response = new BaseResponse();
            try
            {
                var AccDetails = new AccountDetails
                // For demonstration purposes,hardcoding an account balance instead of using an actual database
                {
                    AccountName = "Test",
                    AccountNo = accountNumber,
                    Balance = 2500
                };
                var balance = AccDetails.Balance;
                response.ResponseCode = "00";
                response.ResponseMessage = $"Account balance retrieved successfully";
                return Ok(balance);
            }
            catch (Exception ex)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = $"Error occurred while getting transactions, {ex.Message}";
                return Ok(response);
          }
        }

        [HttpGet("transactions/{accountNumber}")]
        public async Task<ActionResult<List<TransactionDetails>>> GetTransactions(int accountNumber)
        {
            var response = new BaseResponse();
            try
            {
                // For demonstration purposes,hardcoding a list of transactions
                var transactions = new List<TransactionDetails>
                {
                    new TransactionDetails
                    {
                        AccountNo = accountNumber,
                        Amount = (decimal)500.00,
                        Category = "Salary",
                        Description = "Transaction 1",
                        Date = new DateTime(2023, 06, 25)
                    },
                    new TransactionDetails
                    {
                        AccountNo = accountNumber,
                        Amount = (decimal)-50.00,
                        Category = "Entertainment",
                        Description = "Transaction 2",
                        Date = new DateTime(2023, 06, 24)
                    },
                    new TransactionDetails
                    {
                        AccountNo = accountNumber,
                        Amount = (decimal)-200.00,
                        Category = "Groceries",
                        Description = "Transaction 3",
                        Date = new DateTime(2023, 06, 23)
                    }
                };

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = $"Error occurred while getting transactions, {ex.Message}";
                return Ok(response);

            }
        }
        public class BaseResponse
        {
            public string? ResponseCode { get; set; }
            public string? ResponseMessage { get; set; }
            //public object? Data { get; set; }
        }

        public class AccountDetails
        {
            public int AccountNo { get; set; }
            public string? AccountName { get; set; }
            public decimal Balance { get; set; }
        }

        public class TransactionDetails
        {
            public int AccountNo { get; set; }
            public decimal Amount { get; set; }
            public string? Category { get; set; }
            public string? Description { get; set; }
            public DateTime Date { get; set; }
        }

    }
}