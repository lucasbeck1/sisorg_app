namespace api.Services
{
    public class ServiceResult<T>
    {
        public bool Successfull { get; set; }
        public T Data { get; set; }
        public string? Message { get; set; }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { Successfull = true, Data = data };
        public static ServiceResult<T> Fail(string message) => new ServiceResult<T> { Successfull = false, Message = message };
    }
}
