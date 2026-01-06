using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Shared.Models
{
    public class ApiResult<T>
    {
        public ApiResult()
        { }

        private ApiResult(bool succeeded, T result, IEnumerable<string> errors, int? totalRecords)
        {
            Succeeded = succeeded;
            Result = result;
            Errors = errors;
            TotalRecords = totalRecords;
        }

        public IEnumerable<string> Errors { get; set; }
        public T Result { get; set; }
        public bool Succeeded { get; set; }
        public int? TotalRecords { get; set; }

        public static ApiResult<T> Failure(IEnumerable<string> errors)
        {
            return new ApiResult<T>(false, default, errors, null);
        }

        public static ApiResult<T> Success(T result, int? totalRecords = null)
        {
            return new ApiResult<T>(true, result, [], totalRecords);
        }
    }
}