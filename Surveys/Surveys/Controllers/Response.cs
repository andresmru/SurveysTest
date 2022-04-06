namespace Surveys.Controllers
{
    public class Response
    {
        public bool Success { get; set; } = false;

        public int ResultCode { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public object Component { get; set; }
    }
}
