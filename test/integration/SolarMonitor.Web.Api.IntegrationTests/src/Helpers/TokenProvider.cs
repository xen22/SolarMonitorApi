using Microsoft.Extensions.Configuration;

namespace SolarMonitor.Api.IntegrationTests
{
    /// <summary>
    /// Static helper class that returns various pre-created tokens to be used for authenticating
    /// requests against SolarMonitorApi.
    /// The purpose of these tokens is to enable TestHost-based integration tests without having the AuthService
    /// running. For true end-to-end tests, we would not use these but rely on the AuthService to provide the tokens.
    /// 
    /// NOTES:
    ///   - see if there is a way to only allow these tokens to work when the Api is run as part of integration tests.
    ///   - we do store the 2 valid tokens used by tests (UserToken and AdminToken) using Secret Manager 
    ///     since we want to avoid storing them in source control.
    /// </summary>
    public class TokenProvider
    {
        public readonly string UserToken;
        public readonly string AdminToken;
        public const string ExpiredToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4MERFMzQ5QjdFNDhBOEZFQkE4NUY2QTQ0OTU5NDYxM0EyMzlENkUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJpQTNqU2Jma2lvX3JxRjlxUkpXVVlUb2puVzQifQ.eyJuYmYiOjE1MDQ2OTkzNTUsImV4cCI6MTUwNDY5OTM1NiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJTb2xhck1vbml0b3JBcGkiXSwiY2xpZW50X2lkIjoiaW50ZWdyYXRpb24tdGVzdHMiLCJzdWIiOiIyZDFmOTUzZC02Mjk3LTRhMjYtYjA2Ni05MzcwZTMxMThjYWIiLCJhdXRoX3RpbWUiOjE1MDQ2OTkzNTQsImlkcCI6ImxvY2FsIiwicm9sZSI6InVzZXIiLCJzY29wZSI6WyJTb2xhck1vbml0b3JBcGkiXSwiYW1yIjpbInB3ZCJdfQ.Y2nbBXlBWFHXJ1INg7E16W9Pj3KT5OzS_3aYPveVsLknnCSVJbXonC7-I4MAyZxEtTsXHnkeQy2q6JFL52gj_8Bxvw9X_Y209HwSlhwYkbbr9jHY-uZmgswkB6HMu8J0gIhdYZEq2AoTuU3HXwRcCLcNn9Yqm_9phQG5Ij1EUasgEWabItmCzpQaO7isFKKxn1NQZ0D5AN6XofYRzW8rFaAw5HtuRefJpodSqxJOm3AIod006dRlc-5n910C9vuY9l2HlQpNrw8qPar4yp-0B5cyOLXLh-7WpIMC-OyZdJdDoFepGDta49Wu1xb73AZ1rs6pF2xBARPZQr1KlErf3Q";
        public const string InvalidToken = "invalidtokenstring";
        public const string EmptyToken = "";

        public TokenProvider(IConfiguration config)
        {
            UserToken = config["IntegrationTestsUserToken"];
            AdminToken = config["IntegrationTestsAdminToken"];
        }
    }

}