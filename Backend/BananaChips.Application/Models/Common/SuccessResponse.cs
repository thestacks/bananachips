namespace BananaChips.Application.Models.Common;

public class BasicResponse
{
    public static BasicResponse Successful => new BasicResponse { Success = true };
    public static BasicResponse Failure => new BasicResponse { Success = false };
    public bool Success { get; set; }
}