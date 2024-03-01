using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace ASPNetCore.Api.AOP.FilterAop
{
    public class AopResultFilterAttribute : ResultFilterAttribute
    {
        private readonly ILogger _logger;
        public AopResultFilterAttribute(ILogger<AopResultFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///在结果执行之前触发（时间转换，日期转换）
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // 判断返回的结果是否为ObjectResult,即为我们要处理的Json对象
            if (context.Result is ObjectResult result && result.Value != null)
            {
                // 序列化结果，以便进行修改
                string jsongString = JsonConvert.SerializeObject(result.Value);
                var token = JToken.Parse(jsongString);

                //调用ProcessToken对结果进行修改
                ProcessToken(token);

                //将修改后的结果重新赋值给Context.Result
                context.Result = new ObjectResult(token.ToString())
                {
                    StatusCode = result.StatusCode
                };

            }
        }

        /// <summary>
        /// 在结果执行之后触发
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //这个时候结果已经返回了，不允许更新context.Result的结果
        }


        private JToken ProcessToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var obj = new JObject();
                    foreach (var property in ((JObject)token).Properties())
                    {
                        obj.Add(property.Name, ProcessToken(property.Value));
                    }
                    return obj;

                case JTokenType.Array:
                    return new JArray(token.Select(ProcessToken));

                case JTokenType.String:
                    return token.ToString().ToUpperInvariant();

                case JTokenType.Date:
                    return ((DateTime)token).ToString("o");

                default:
                    return token;
            }
        }



    }
}
