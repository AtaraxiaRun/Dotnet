#region 基础Middleware 中间件用法
#if false

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
//短小精悍的中间件
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
#endif
#endregion

#region 带依赖注入的中间件
#if false

using NetCore.Api.Middleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILog, Log>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseMyCustomMiddleware(); //具有构造参数的注入中间件

/*
 输出：
进入了有参的构造函数中间件
王锐日志:138
 */

app.MapGet("/", () => "Hello World!");

app.Run();
#endif
#endregion

#region 通过接口创建的中间件
using NetCore.Api.Middleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyInterfaceMiddleware>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseMyInterfaceMiddleware(); //通过接口创建的中间件


app.Run();
#endregion

#region 使用Map控制指定的访问链接，访问特定的中间件，比如当访问http://localhost:5009/map1时会触发HandleMapTest1中间件，输出：Map Test 1
#if false

using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/map1", HandleMapTest1);
app.Map("/map1/seg1", HandleMultiSeg);  //也可以是多个段
app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch); //满足什么条件进行触发，localhost:1234/?branch=main	,感觉可以做参数的校验
app.Map("/map2", HandleMapTest2);


app.UseWhen(context => context.Request.Query.ContainsKey("branch"),
    appBuilder => HandleBranchAndRejoin(appBuilder));   //m满足条件后加入主管道

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
static void HandleMultiSeg(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 1");
    });
}

static void HandleBranch(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        var branchVer = context.Request.Query["branch"];
        await context.Response.WriteAsync($"Branch used = {branchVer}");
    });
}

void HandleBranchAndRejoin(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>(); 

    app.Use(async (context, next) =>
    {
        var branchVer = context.Request.Query["branch"];
        logger.LogInformation("Branch used = {branchVer}", branchVer);

        // Do work that doesn't write to the Response.
        await next();
        // Do other work that doesn't write to the Response.
    });
}

#endif
#endregion
#region 速率控制器
#if false
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
 
var builder = WebApplication.CreateBuilder(args);

var concurrencyPolicy = "Concurrency";
var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

builder.Services.AddRateLimiter(_ => _
    .AddConcurrencyLimiter(policyName: concurrencyPolicy, options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = myOptions.QueueLimit;
    }));

var app = builder.Build();

app.UseRateLimiter();

static string GetTicks() => (DateTime.Now.Ticks & 0x11111).ToString("00000");

app.MapGet("/", async () =>
{
    await Task.Delay(500);
    return Results.Ok($"Concurrency Limiter {GetTicks()}");

}).RequireRateLimiting(concurrencyPolicy);

app.Run();

public class MyRateLimitOptions
{
    public const string MyRateLimit = "MyRateLimit";
    public int PermitLimit { get; set; } = 100;  //最大并发请求
    public int Window { get; set; } = 10;
    public int ReplenishmentPeriod { get; set; } = 2;
    public int QueueLimit { get; set; } = 2;
    public int SegmentsPerWindow { get; set; } = 8;
    public int TokenLimit { get; set; } = 10;
    public int TokenLimit2 { get; set; } = 20;
    public int TokensPerPeriod { get; set; } = 4;
    public bool AutoReplenishment { get; set; } = false;
}
#endif
#endregion