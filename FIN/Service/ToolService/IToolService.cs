using FIN.Enums;

namespace FIN.Service.ToolService
{
    public interface IToolService
    {
        // Creates a response message and returns it
        Dictionary<string, object> Response(Result result, object message);

        // Validates password
        public bool ValidatePassword(string password);

        // Validates phone number
        public bool ValidatePhoneNumber(string num);

        // Validates email
        public bool ValidateEmail(string email);


    }
}
