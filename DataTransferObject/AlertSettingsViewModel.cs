using System;
namespace FinanceChecker.DataTransferObject
{
    public class AlertSettingsViewModel
    {
        public bool LowBalanceAlertEnabled { get; set; }

        public decimal LowBalanceThreshold { get; set; }


        public bool HighBalanceAlertEnabled { get; set; }

        public decimal HighBalanceThreshold { get; set; }

        public bool OverspendingAlertEnabled { get; set; }

        //public decimal BudgetAmount { get; set; }
      
        public bool DueDateReminderAlertEnabled { get; set; }

        public bool IncomeDepositedAlertEnabled { get; set; }


        public bool TargetAmountReachedAlertEnabled { get; set; }

    }
}
