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
app.UseExceptionHandling(); //�쳣����  //�ŵ��ʼ�����Բ������������м�����쳣
app.UseMyMiddleWare(); //�Զ����м��
app.UseLogging();//д����־
app.UseAuthorizationMiddleware();//Ȩ����֤
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
