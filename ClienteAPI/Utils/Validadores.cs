using System.Net.Mail;

namespace ClienteAPI.Utils
{
    public static class Validadores
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var m = new MailAddress(email);
                return m.Address == email;
            }
            catch { return false; }
        }

        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) 
                return false;

            var numbers = new string(cpf.Where(char.IsDigit).ToArray());
            return numbers.Length == 11;
        }

        public static bool IsValidRg(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg)) return false;
            var numbers = new string(rg.Where(char.IsLetterOrDigit).ToArray());
            return numbers.Length >= 5 && numbers.Length <= 12;
        }

        public static bool IsValidCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return false;
            var numbers = new string(cep.Where(char.IsDigit).ToArray());
            return numbers.Length == 8;
        }
    }
}