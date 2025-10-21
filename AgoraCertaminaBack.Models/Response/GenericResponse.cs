using System.Net;

namespace SensusAPI.Models.Response
{
    /// <summary>
    /// This class contains the structure of a generic response 
    /// </summary>
    public class GenericResponse<T>
    {
        /// <summary>
        /// Contains the HttpStatusCode of the request
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Contains result type of the request
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// Contains the T Object response
        /// </summary>
        public T? Response { get; set; }

        /// <summary>
        /// List that contains errors from the request
        /// </summary>
        public List<string> Errors { get; set; }

        public GenericResponse()
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
            ResponseType = ResponseTypeEnum.Error.ToString();
            Errors = new List<string>();
        }
    }
}
