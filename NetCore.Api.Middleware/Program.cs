#region ����Middleware �м���÷�
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
//�м��ѧϰ��https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0
//�м����ִ��˳��ͼƬ����https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/index/_static/middleware-pipeline.svg?view=aspnetcore-8.0
app.UseExceptionHandling(); //�쳣����  //�ŵ��ʼ�����Բ������������м�����쳣

//app.Run�ն��м��,�������˺󣬴����������ˣ��������е��м��������ִ��[һ��ŵ����ִ��]
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});
app.Use(async (context, next) =>
    {
        await Console.Out.WriteLineAsync("û�е���await next() Ҳ������������м����");
        await next.Invoke();
    });
app.UseRouting();
//��С�������м��
app.Use(async (context, next) =>
{
    // await context.Response.WriteAsync("Hello world Use!"); //��Ӧ��ֻ��д��һ�Σ���ΪHTTPЭ�鲻�����ڷ�����Ӧ��֮���ٸ�����Ӧͷ����Ϣ
   await Console.Out.WriteLineAsync("Hello world Use!");
   await next(); // ���ùܵ��е���һ���м����Ҳ���Բ����ã�ֱ���ùܵ���·��ֹͣ���С�
});

app.UseMyMiddleWare(); //�Զ����м��
app.UseLogging();//д����־
app.UseAuthorizationMiddleware();//Ȩ����֤
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
#endif
#endregion

#region ������ע����м��
#if false

using NetCore.Api.Middleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILog, Log>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseMyCustomMiddleware(); //���й��������ע���м��

/*
 �����
�������вεĹ��캯���м��
������־:138
 */

app.MapGet("/", () => "Hello World!");

app.Run();
#endif
#endregion

#region ͨ���ӿڴ������м��
using NetCore.Api.Middleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyInterfaceMiddleware>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseMyInterfaceMiddleware(); //ͨ���ӿڴ������м��


app.Run();
#endregion

#region ʹ��Map����ָ���ķ������ӣ������ض����м�������統����http://localhost:5009/map1ʱ�ᴥ��HandleMapTest1�м���������Map Test 1
#if false

using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/map1", HandleMapTest1);
app.Map("/map1/seg1", HandleMultiSeg);  //Ҳ�����Ƕ����
app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch); //����ʲô�������д�����localhost:1234/?branch=main	,�о�������������У��
app.Map("/map2", HandleMapTest2);


app.UseWhen(context => context.Request.Query.ContainsKey("branch"),
    appBuilder => HandleBranchAndRejoin(appBuilder));   //m����������������ܵ�

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
#region ���ʿ�����
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
    public int PermitLimit { get; set; } = 100;  //��󲢷�����
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