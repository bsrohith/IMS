using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class ErrorService
    {
        private readonly List<string> _errors = new();

        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        public event Action? OnErrorsUpdated;

        public void LogError(Exception exception)
        {
            var errorMessage = $"{DateTime.Now}: {exception.Message}";
            _errors.Add(errorMessage);

            Console.Error.WriteLine(errorMessage); // Logs to browser console
            OnErrorsUpdated?.Invoke(); // Notify listeners (like Error Page)
        }

        public void ClearErrors()
        {
            _errors.Clear();
            OnErrorsUpdated?.Invoke();
        }
    }
}
