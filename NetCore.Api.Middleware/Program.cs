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

#region 使用Map控制指定的访问链接，访问特定的中间件，比如当访问http://localhost:5009/map1时会触发HandleMapTest1中间件，输出：Map Test 1
#if true

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
