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
//中间件的执行顺序（图片）：https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/index/_static/middleware-pipeline.svg?view=aspnetcore-8.0
app.UseExceptionHandling(); //异常处理  //放到最开始，可以捕获所有下面中间件的异常

//app.Run终端中间件,遇到我了后，代表程序结束了，后面所有的中间件都不会执行[一般放到最后执行]
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});
app.Use(async (context, next) =>
    {
        await Console.Out.WriteLineAsync("没有调用await next() 也可以往下面的中间件走");
        await next.Invoke();
    });
app.UseRouting();
app.Use(async (context, next) =>
{
    // await context.Response.WriteAsync("Hello world Use!"); //响应体只能写入一次，因为HTTP协议不允许在发送响应体之后再更改响应头部信息
   await Console.Out.WriteLineAsync("Hello world Use!");
   await next(); // 调用管道中的下一个中间件【也可以不调用，直接让管道短路，停止运行】
});

app.UseMyMiddleWare(); //自定义中间件
app.UseLogging();//写入日志
app.UseAuthorizationMiddleware();//权限认证
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();

#region 使用Map控制指定的访问链接，访问特定的中间件，比如当访问http://localhost:5009/map1时会触发HandleMapTest1中间件，输出：Map Test 1
#if false
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/map1", HandleMapTest1);

app.Map("/map2", HandleMapTest2);

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from non-Map delegate.");
});

app.Run();

static void HandleMapTest1(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 1");
    });
}

static void HandleMapTest2(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 2");
    });
}
#endif
#endregion
