using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceChecker.Models
{
    public class UserAlertSettings
    {
        [Key]
        public int AlertID { get; set; }

        public Guid UserID { get; set; }

        public bool LowBalanceAlertEnabled { get; set; }

        public decimal LowBalanceThreshold { get; set; }

        public bool HighBalanceAlertEnabled { get; set; }

        public decimal HighBalanceThreshold { get; set; }

        public bool OverspendingAlertEnabled { get; set; }


        public bool DueDateReminderAlertEnabled { get; set; }

        public bool IncomeDepositedAlertEnabled { get; set; }


        public bool TargetAmountReachedAlertEnabled { get; set; }


    }

}



