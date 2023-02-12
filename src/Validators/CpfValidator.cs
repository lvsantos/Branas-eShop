using System.Text.RegularExpressions;

namespace eShop.Api.Validators;

public class CpfValidator
{
    private const int LENGTH = 11;

    public static bool Validate(string? cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            return false;
        }
        cpf = Sanitize(cpf);
        if (cpf.Length != LENGTH)
        {
            return false;
        }
        bool allCharactersTheSame = cpf.All(c => c == cpf[0]);
        if (allCharactersTheSame)
        {
            return false;
        }
        int firstCheckDigit = CalculateFirstCheckDigit(cpf);
        int secondCheckDigit = CalculateSecondCheckDigit(cpf, firstCheckDigit);
        string cpfCheckDigits = GetCheckDigits(cpf);
        string calculatedCheckDigits = "" + firstCheckDigit + "" + secondCheckDigit;
        return cpfCheckDigits == calculatedCheckDigits;

        string Sanitize(string fiscalCodeNumber)
        {
            var pattern = @"[^\d]+";
            var replacement = "";
            string result = Regex.Replace(fiscalCodeNumber, pattern, replacement);
            return result;
        }
        int CalculateFirstCheckDigit(string cpf)
        {
            var cpfWithNoCheckDigits = RemoveCheckDigits(cpf);
            int sum = CalculateCheckDigitSum(cpfWithNoCheckDigits);
            return CalculateCheckDigit(sum);
        }
        int CalculateSecondCheckDigit(string cpf, int firstVerifyDigit)
        {
            var cpfWithNoCheckDigits = RemoveCheckDigits(cpf);
            int sum = CalculateCheckDigitSum(cpfWithNoCheckDigits + firstVerifyDigit);
            return CalculateCheckDigit(sum);
        }
        int CalculateCheckDigitSum(string cpfWithoutCheckDigits)
        {
            var sum = 0;
            int factor = cpfWithoutCheckDigits.Length + 1;
            foreach (char charactere in cpfWithoutCheckDigits)
            {
                int digit = int.Parse(charactere.ToString());
                sum += factor-- * digit;
            }
            return sum;
        }
        int CalculateCheckDigit(int sum)
        {
            int rest = sum % LENGTH;
            return rest < 2 ? 0 : LENGTH - rest;
        };
        string RemoveCheckDigits(string cpf) => cpf.Substring(0, LENGTH - 2);
        string GetCheckDigits(string cpf) => cpf.Substring(LENGTH - 2, 2);
    }
}