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
app.UseExceptionHandling(); //�쳣����  //�ŵ��ʼ�����Բ������������м�����쳣

//app.Run�ն��м��,�������˺󣬴����������ˣ��������е��м��������ִ��[һ��ŵ����ִ��]
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});

app.Use(async (context, next) =>
{
    // await context.Response.WriteAsync("Hello world Use!"); //��Ӧ��ֻ��д��һ�Σ���ΪHTTPЭ�鲻�����ڷ�����Ӧ��֮���ٸ�����Ӧͷ����Ϣ
    await Console.Out.WriteLineAsync("Hello world Use!");
    await next(); // ���ùܵ��е���һ���м��
});

app.UseMyMiddleWare(); //�Զ����м��
app.UseLogging();//д����־
app.UseAuthorizationMiddleware();//Ȩ����֤
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
