namespace Domain.Utils
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static bool ValidateCPF(string cpf)
        {
            // Remove spaces and special characters
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Check if the length is exactly 11 digits
            if (cpf.Length != 11)
                return false;

            // Check for repeated digit patterns (e.g., 11111111111)
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Calculation for the first verification digit
            int[] firstDigitMultipliers = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * firstDigitMultipliers[i];
            }

            int remainder = sum % 11;
            string verificationDigit = (remainder < 2) ? "0" : (11 - remainder).ToString();

            // Calculation for the second verification digit
            int[] secondDigitMultipliers = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * secondDigitMultipliers[i];
            }

            remainder = sum % 11;
            verificationDigit += (remainder < 2) ? "0" : (11 - remainder).ToString();

            // Check if the calculated verification digits match the ones in the CPF
            return cpf.EndsWith(verificationDigit);
        }
    }
}
