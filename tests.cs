using NUnit.Framework;

[TestFixture]
public class SecurityTests
{
    [Test]
    public void TestForSQLInjection()
    {
        string maliciousInput = "'; DROP TABLE Users; --";
        string sanitized = InputSanitizer.SanitizeUsername(maliciousInput);

        Assert.IsFalse(sanitized.Contains("DROP"), "SQL Injection attempt should be sanitized.");
    }

    [Test]
    public void TestForXSS()
    {
        string maliciousInput = "<script>alert('XSS');</script>";
        string sanitized = InputSanitizer.SanitizeUsername(maliciousInput);

        Assert.IsFalse(sanitized.Contains("<script>"), "XSS attempt should be sanitized.");
    }
}
