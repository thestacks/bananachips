namespace Common.Validation.Utilities;

public static class NipChecker
{
    public static bool Check(string candidate)
    {
        if (string.IsNullOrEmpty(candidate))
            return false;
        
        var nip = candidate;
        nip = nip.Replace("-", string.Empty);

        if (nip.Length != 10 || nip.Any(chr => !char.IsDigit(chr)))
            return false;

        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7, 0 };
        var sum = nip.Zip(weights, (digit, weight) => (digit - '0') * weight).Sum();

        return sum % 11 == nip[9] - '0';
    }
}