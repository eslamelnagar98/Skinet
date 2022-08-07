namespace API.Errors;
public class ApiValidationErrorResponse : ApiResponse
{
    public List<string> Errors { get; set; }
    public ApiValidationErrorResponse()
        : base(400)
    {

    }
}
