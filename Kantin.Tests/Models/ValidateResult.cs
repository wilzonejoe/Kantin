using System.Text;

namespace Kantin.Tests.Models
{
    public class ValidateResult
    {
        private StringBuilder _messageBuilder;

        private bool _success;
        public bool Success 
        {
            get => _success;
            set
            {
                if (!_success)
                    return;

                _success = value;
            }
        }

        public string Message => _messageBuilder?.ToString();

        public ValidateResult() 
        {
            _success = true;
            _messageBuilder = new StringBuilder(); 
        }

        public void AppendMessage(string message) => _messageBuilder.AppendLine(message);
    }
}
