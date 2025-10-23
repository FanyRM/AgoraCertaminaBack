using Microsoft.AspNetCore.Mvc;
using ROP;
using System.Runtime.ExceptionServices;

namespace AgoraCertaminaBack.Models.Response
{
    public static class GenericReponseExtensions
    {
        public static ObjectResult ToActionResult<T>(this GenericResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = (int)response.HttpStatusCode,
                Value = response    
            };
        }

        public static async Task<ObjectResult> ToActionResult<T>(this Task<GenericResponse<T>> result)
        {
            try
            {
                return (await result).ToActionResult();
            }
            catch (Exception source)
            {
                ExceptionDispatchInfo.Capture(source).Throw();
                throw;
            }
        }
    }
}
