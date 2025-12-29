namespace CommunityEventManagementSystem.Services;

public sealed class ServiceResult
{
    public bool Ok { get; init; }
    public string Message { get; init; } = "";
    public static ServiceResult Success(string message = "") => new() { Ok = true, Message = message };
    public static ServiceResult Fail(string message) => new() { Ok = false, Message = message };
}
