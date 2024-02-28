using NetCore.Api.Middleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//中间件学习：https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0
app.UseExceptionHandling(); //异常处理  //放到最开始，可以捕获所有下面中间件的异常

//app.Run终端中间件,遇到我了后，代表程序结束了，后面所有的中间件都不会执行[一般放到最后执行]
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});

app.Use(async (context, next) =>
{
    // await context.Response.WriteAsync("Hello world Use!"); //响应体只能写入一次，因为HTTP协议不允许在发送响应体之后再更改响应头部信息
    await Console.Out.WriteLineAsync("Hello world Use!");
    await next(); // 调用管道中的下一个中间件
});

app.UseMyMiddleWare(); //自定义中间件
app.UseLogging();//写入日志
app.UseAuthorizationMiddleware();//权限认证
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
