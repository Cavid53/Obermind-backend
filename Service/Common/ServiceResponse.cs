using System.Collections.Generic;

namespace Service.Common
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
        public string Error { get; set; } = null;
        public List<string> ErrorMessages { get; set; } = null;
    }
}
