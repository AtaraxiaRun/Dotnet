namespace ASP.NET.CORE.API.JwtHandler.Models
{
    /// <summary>
    /// 用于接受配置数据实体类型。 
    /// </summary>
    public sealed class JWTTokenOption
    {
        /// <summary>
        /// 获取或者设置接受者。
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// 获取或者设置加密 key。
        /// </summary>
        public string? SecurityKey { get; set; }

        /// <summary>
        /// 获取或者设置发布者
        /// </summary>
        public string? Issuer { get; set; }
    }
}
