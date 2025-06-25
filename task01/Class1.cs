namespace task01;

public static class StringExtensions
{ 
    public static bool IsPalindrome(this string input)
    {
        if(string.IsNullOrEmpty(input)) return false;
        input = new String(input.ToLower().Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).ToArray());
        return input == new string(input.Reverse().ToArray());
    }
}
