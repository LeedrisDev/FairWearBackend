namespace BrandAndProductDatabase.API.Models.Response;

/// <summary>Model for error response</summary>
public class ErrorResponse
{
    /// <summary>Message of the error</summary>
    public string Message { get; set; } = string.Empty;
}