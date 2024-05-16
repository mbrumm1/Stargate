using System.Net;

namespace StargateAPI.Controllers
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Successful";
        public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;

        public static BaseResponse InternalServerError(string message) => new BaseResponse
        {
            Success = false,
            Message = message,
            ResponseCode = (int)HttpStatusCode.InternalServerError
        };
    }
}