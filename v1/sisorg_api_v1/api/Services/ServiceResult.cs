namespace api.Services
{
    public class ServiceResult<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public string message { get; set; }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { success = true, data = data };
        public static ServiceResult<T> Fail(string message) => new ServiceResult<T> { success = false, message = message };
    }
}
