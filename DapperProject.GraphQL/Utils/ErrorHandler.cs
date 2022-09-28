namespace DapperProject.GraphQL.Utils
{
    internal class ErrorHandler : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error.WithMessage(error.Exception.Message);
        }
    }
}
