using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FileServer.Application.Dtos
{
    /// <summary>
    /// 表示强类型的响应实体基类
    /// </summary>
    public class ResponseDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 可以在任何地方输出具有相同响应头格式的 响应对象。【正常输出、异常、未授权等】
    /// DTO不可继承自该对象（因为会导致页面的数据，层级较深的全带IsSuccess，Message这些无用信息！）
    /// </summary>
    public sealed class ResponseDto
    {
        /// <summary>
        /// 响应内容类型（application/json）
        /// </summary>
        public static string ContentType => "application/json";

        /// <summary>
        /// 表示全局的序列化设置对象 
        /// </summary>
        public static JsonSerializerSettings SerializerSettings => new JsonSerializerSettings()
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// 操作成功，返回提示消息
        /// </summary>
        public static ResponseDto<string> Success(string message)
        {
            return new ResponseDto<string> { Status = ResponseStatus.Ok, Message = message, Data = null };
        }

        /// <summary>
        /// 查询成功，并返回具体数据
        /// </summary>
        public static ResponseDto<T> Success<T>(string message, T data)
        {
            return new ResponseDto<T> { Status = ResponseStatus.Ok, Message = message, Data = data };
        }

        /// <summary>
        /// 执行失败，并返回具体数据
        /// </summary>
        public static ResponseDto<T> Error<T>(string message, T data)
        {
            return new ResponseDto<T> { Status = ResponseStatus.InternalServerError, Message = message, Data = data };
        }

        /// <summary>
        /// 访问未授权，返回提示消息【需要重新跳转到登陆界面进行授权，只是提示消息不同而已，未授权/令牌过期/令牌错误】
        /// </summary>
        public static ResponseDto<string> Unauthorized(string message)
        {
            return new ResponseDto<string> { Status = ResponseStatus.Unauthorized, Message = message, Data = null };
        }

        /// <summary>
        /// 访问被禁止（用户过期）
        /// </summary>
        public static ResponseDto<string> Forbidden(string message)
        {
            return new ResponseDto<string> { Status = ResponseStatus.Forbidden, Message = message, Data = null };
        }

        /// <summary>
        /// 店铺信息未初始化
        /// </summary>
        public static ResponseDto<string> NotInitialized(string message)
        {
            return new ResponseDto<string> { Status = ResponseStatus.NotInitialized, Message = message, Data = null };
        }

        /// <summary>
        /// 生成新的相应实体类
        /// </summary>
        public static ResponseDto<string> New(ResponseStatus status, string message)
        {
            return new ResponseDto<string> { Status = status, Message = message, Data = null };
        }
    }
    /// <summary>
    /// 响应状态码
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        Ok = 200,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 禁止访问
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 没有页面
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 执行错误
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 自定义状态码：信息未初始化
        /// </summary>
        NotInitialized = 600,
    }
}