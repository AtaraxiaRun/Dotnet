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

#region ʹ��Map����ָ���ķ������ӣ������ض����м�������統����http://localhost:5009/map1ʱ�ᴥ��HandleMapTest1�м���������Map Test 1
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
