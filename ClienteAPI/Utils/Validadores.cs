using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ClienteAPI.Utils
{
    public static partial class Validadores
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
            
            var n = new string(cpf.Where(char.IsDigit).ToArray());
            if (n.Length != 11) 
                return false;
            
            if (n.All(c => c == n[0])) 
                return false;

            var numbers = new string(cpf.Where(char.IsDigit).ToArray());
            return numbers.Length == 11;
        }

        public static bool IsValidRg(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg)) return false;

            rg = new string(rg.Where(char.IsLetterOrDigit).ToArray());

            if (rg.Length is < 5 or > 12)
                return false;

            var regex = RegexRg();
            return regex.IsMatch(rg);
        }

        public static bool IsValidCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return false;

            cep = new string(cep.Where(char.IsDigit).ToArray());

            if (cep.Length != 8)
                return false;

            var regex = RegexCep();
            return regex.IsMatch(cep);
        }

        [GeneratedRegex(@"^[A-Za-z0-9]{5,12}$")]
        private static partial Regex RegexRg();
        
        [GeneratedRegex(@"^\d{8}$")]
        private static partial Regex RegexCep();
    }
}