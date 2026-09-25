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


[TestFixture]
public class AuthTests
{
    private AuthService authService;

    [SetUp]
    public void Setup()
    {
        authService = new AuthService("YourConnectionStringHere");
        authService.RegisterUser("adminUser", "admin@example.com", "SecurePass123", "admin");
        authService.RegisterUser("normalUser", "user@example.com", "UserPass456", "user");
    }

    [Test]
    public void TestInvalidLogin()
    {
        bool result = authService.AuthenticateUser("adminUser", "WrongPassword");
        Assert.IsFalse(result, "Invalid login should fail.");
    }

    [Test]
    public void TestValidLogin()
    {
        bool result = authService.AuthenticateUser("adminUser", "SecurePass123");
        Assert.IsTrue(result, "Valid login should succeed.");
    }

    [Test]
    public void TestUnauthorizedAccess()
    {
        bool result = authService.AuthorizeUser("normalUser", "admin");
        Assert.IsFalse(result, "Normal user should not access admin features.");
    }

    [Test]
    public void TestAuthorizedAccess()
    {
        bool result = authService.AuthorizeUser("adminUser", "admin");
        Assert.IsTrue(result, "Admin user should access admin features.");
    }
}

