using System.Text.RegularExpressions;

public static class InputSanitizer
{
    public static string SanitizeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return string.Empty;

        // Remove any non-alphanumeric characters
        return Regex.Replace(username, @"[^a-zA-Z0-9]", "");
    }

    public static string SanitizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return string.Empty;

        // Basic email validation
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (Regex.IsMatch(email, emailPattern))
        {
            return email;
        }
        return string.Empty;
    }
}

using System.Web;

public static class OutputSanitizer
{
    public static string EscapeForHtml(string input)
    {
        return HttpUtility.HtmlEncode(input);
    }
}
