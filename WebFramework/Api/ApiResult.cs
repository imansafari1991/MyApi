﻿using Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResult(bool isSuccess,ApiResultStatusCode statusCode,string message=null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message??statusCode.ToDisplay();
        }
        #region Implicit Operators

        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }
        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound);
        }
        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }
        public static implicit operator ApiResult(BadRequestObjectResult result)
        {

            var message = result.Value.ToString();
            if(result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.Success,result.Content);
        }
        #endregion
    }
    public class ApiResult<TData> : ApiResult
        where TData:class
    {


        public TData Data { get; set; }

        public ApiResult(bool isSuccess,ApiResultStatusCode statusCode,TData data,string message=null):base(isSuccess,statusCode,message)
        {
            Data = data;
        }
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
             
        }
        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success,null);
        }
        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound,null);
        }
        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest,null);
        }
        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {

            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest,null);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.Success,null, result.Content);
        }
    }
    public enum ApiResultStatusCode
    {
        [Display(Name="عملیات با موفقیت انجام شد")]
        Success=0,
        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError =1,
        [Display(Name = "پارامترهای ارسالی  معتبر نیستید")]
        BadRequest =2,
        [Display(Name = "یافت نشد")]
        NotFound =3,
        [Display(Name = "لیست خالی است")]
        ListEmpty =4
    }
}
