using MainProject.Core.Enums;

namespace MainProject.Framework.Models
{
    /// <summary>
    /// Return response with data
    /// </summary>
    /// <typeparam name="T">Class T</typeparam>
    public class BaseResponseModel<T>
    {
        /// <summary>
        /// Result code
        /// </summary>
        public HttpStatusCodeCollection Code { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Model validation display error for field
        /// </summary>
        public string FieldError { get; set; }

        /// <summary>
        /// Some data model
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// Return response without data
    /// </summary>
    /// <typeparam name="T">Class T</typeparam>
    public class BaseResponseModel
    {
        /// <summary>
        /// Result code
        /// </summary>
        public HttpStatusCodeCollection Code { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }
    }
}
