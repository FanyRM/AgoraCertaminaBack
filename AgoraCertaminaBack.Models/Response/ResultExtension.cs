using ROP;
using System.Net;
using System.Runtime.ExceptionServices;

namespace AgoraCertaminaBack.Models.Response
{
    public static class ResultExtensions
    {
        public static GenericResponse<T> ToGenericResponse<T>(this Result<T> result)
        {
            return new GenericResponse<T>
            {
                HttpStatusCode = result.HttpStatusCode,
                ResponseType = GetResponseType(result).ToString(),
                Response = result.Success ? result.Value : default,
                Errors = result.Errors.Select(e => e.Message).ToList() ?? new List<string>()
            };
        }

        public static async Task<GenericResponse<T>> ToGenericResponse<T>(this Task<Result<T>> result)
        {
            try
            {
                return (await result).ToGenericResponse();
            }
            catch (Exception source)
            {
                ExceptionDispatchInfo.Capture(source).Throw();
                throw;
            }
        }

        private static ResponseTypeEnum GetResponseType<T>(Result<T> result)
        {
            if (result.Errors.Any() && result.HttpStatusCode != HttpStatusCode.Accepted)
            {
                return ResponseTypeEnum.Error;
            }
            else if (result.HttpStatusCode == HttpStatusCode.Accepted || result.Value == null)
            {
                return ResponseTypeEnum.Warning;
            }
            else
            {
                return ResponseTypeEnum.Success;
            }
        }
    }
}
