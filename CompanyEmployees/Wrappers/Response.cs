namespace CompanyEmployees.Wrappers
{
    public class Response<T>
    {
        public Response(string message, string [] errors)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string [] Errors { get; set; }
        public string Message { get; set; }
    }
}