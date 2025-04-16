using RestaurantLibrary.Helpers;
using RestaurantLibrary.Helpers.Enums;
using System;
using System.Threading.Tasks;


namespace RestaurantLibrary.Models.Transactions
{
    public class BalanceSystem
    {
        private static readonly Lazy<BalanceSystem> _instance = new Lazy<BalanceSystem>(() => new BalanceSystem());
        public static BalanceSystem Instance => _instance.Value;

        private TransactionLog dbTransactionLog = new TransactionLog();
        private CassaLog dbCassaLog = new CassaLog();
        private EntryLog dbEntryLog = new EntryLog();
        private SalaryLog dbSalaryLog = new SalaryLog();
        private Balance dbBalance = new Balance();


        public async Task<double> GetCurrentBalance()
        {
            return (await dbBalance.OnLoadAsync()).budget_money;
        }

        private async Task<double> CalculateNewBalance(TransactionLog transaction)
        {
            var currentBalance = await GetCurrentBalance();
            return transaction.transaction_type == EnumTransactionType.Доход
                ? currentBalance + transaction.transaction_amount
                : currentBalance - transaction.transaction_amount;
        }


        // Основные методы для работы с транзакциями
        public async Task AddTransaction(TransactionLog transaction)
        {
            if (transaction.transaction_type == EnumTransactionType.Доход
                            || transaction.transaction_type == EnumTransactionType.Расход)
            {
                transaction.transaction_balance = await CalculateNewBalance(transaction);
                await ApplyTransaction(transaction);
            }

            await dbTransactionLog.OnInsertTransactionAsync(transaction);
        }

        public async Task DeleteTransaction(TransactionLog transaction)
        {
            await ReverseTransaction(transaction);
            await dbTransactionLog.OnDeleteTransactionAsync(transaction.transaction_id);
        }

        public async Task UpdateTransaction(TransactionLog updatedTransaction)
        {
            var existing = await dbTransactionLog.OnSelectTransactionAsync(updatedTransaction.transaction_id);
            if (existing != null)
            {
                await ReverseTransaction(existing);
                await ApplyTransaction(updatedTransaction);
            }
        }

        private async Task ApplyTransaction(TransactionLog transaction)
        {
            var currentBalance = await GetCurrentBalance();

            if (transaction.transaction_type == EnumTransactionType.Доход)
                currentBalance += transaction.transaction_amount;
            else
                currentBalance -= transaction.transaction_amount;

            await dbBalance.OnUpdateBalance(currentBalance);
        }

        private async Task ReverseTransaction(TransactionLog transaction)
        {
            var currentBalance = await GetCurrentBalance();

            if (transaction.transaction_type == EnumTransactionType.Доход)
                currentBalance -= transaction.transaction_amount;
            else
                currentBalance += transaction.transaction_amount;

            await dbBalance.OnUpdateBalance(currentBalance);
        }

        public async Task AddTransactionOperation(EnumTransactionType paymentType, double amount, int userId, string description = "")
        {
            var transaction = new TransactionLog
            {
                transaction_type = paymentType,
                transaction_amount = amount,
                transaction_description = description,
                transaction_date = DateTimeHelper.GetCurrentDateTimeString(),
                transaction_user = userId,
            };

            await AddTransaction(transaction);
        }

        // Методы для работы с CassaLog
        public async Task AddCassaOperation(EnumCassaOperationType operationType, double amount, int userId, int cassaId = 0, int cardId = 0, string description = "")
        {
            var cassaLog = new CassaLog
            {
                transaction_type = operationType == EnumCassaOperationType.Пополнение ?
                    EnumTransactionType.Расход :
                    EnumTransactionType.Доход,
                transaction_description = operationType == EnumCassaOperationType.Пополнение ?
                    "Пополнение кассы" :
                    "Выемка кассы",
                transaction_cassa_operation = operationType,
                transaction_amount = amount,
                transaction_cassa_description = description,
                transaction_cassa = cassaId,
                transaction_card = cardId,
                transaction_date = DateTimeHelper.GetCurrentDateTimeString(),
                transaction_user = userId,
            };

            await AddTransaction(cassaLog);
        }

        public async Task AddEntryOperation(double amount, int entryId, int userId, string description = "")
        {
            var entryLog = new EntryLog
            {
                transaction_type = EnumTransactionType.Расход, // Приход всегда уменшает баланс
                transaction_amount = amount,
                transaction_entry_description = description,
                transaction_entry = entryId,
                transaction_description = $"Приход №{entryId}",
                transaction_date = DateTimeHelper.GetCurrentDateTimeString(),
                transaction_user = userId,
            };

            await AddTransaction(entryLog);
        }

        public async Task DeleteEntryOperations(int entryId)
        {
            var entryTransactions = await dbEntryLog.OnSelectEntryTransactionsAsync(entryId);

            foreach (var transaction in entryTransactions)
            {
                await DeleteTransaction(transaction);
            }
        }

        // Методы для работы с зарплатами (SalaryLog)
        public async Task AddSalaryOperation(EnumSalaryLogType operationType, double amount, int userSalary, int userId, string description = "")
        {
            var salaryLog = new SalaryLog
            {
                transaction_type = (operationType == EnumSalaryLogType.Оплата || operationType == EnumSalaryLogType.Предоплата) ?
                                    EnumTransactionType.Расход : 0,
                transaction_salary_type = operationType,
                transaction_amount = amount,
                transaction_salary_description = description,
                transaction_description = description,
                transaction_date = DateTimeHelper.GetCurrentDateTimeString(),
                transaction_salary_user = userSalary,
                transaction_user = userId,
            };

            await AddTransaction(salaryLog);
        }


        //public void DeleteSalaryOperations(int userId)
        //{
        //    var salaryTransactions = _transactions
        //        .OfType<SalaryLog>()
        //        .Where(t => t.transaction_user == userId)
        //        .ToList();

        //    foreach (var transaction in salaryTransactions)
        //    {
        //        DeleteTransaction(transaction.transaction_id);
        //    }
        //}
    }
}
