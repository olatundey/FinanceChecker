using System;
namespace FinanceChecker.DataTransferObject
{
    // account information needed in the view for create transaction.
    public class AlertSettingsViewModel
    {
        public bool LowBalanceAlertEnabled { get; set; }

        public decimal LowBalanceThreshold { get; set; }

        // public decimal TotalBalance { get; set; }

        public bool HighBalanceAlertEnabled { get; set; }

        public decimal HighBalanceThreshold { get; set; }

        public bool OverspendingAlertEnabled { get; set; }

        //public decimal BudgetAmount { get; set; }

        //public bool UnusualExpensesAlertEnabled { get; set; }

        public bool DueDateReminderAlertEnabled { get; set; }

        public bool IncomeDepositedAlertEnabled { get; set; }

        //public bool SavingsGoalProgressAlertEnabled { get; set; }

        //public bool TargetAmountReachedAlertEnabled { get; set; }

        //public bool SuspiciousActivityAlertEnabled { get; set; }

        // public bool LoginFromNewDeviceAlertEnabled { get; set; }
    }
}
