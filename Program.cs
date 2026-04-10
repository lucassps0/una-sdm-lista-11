using GlobalBankApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Diz para a API que vamos usar controllers
builder.Services.AddControllers();

// Liga o Swagger, que é a tela para testar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra o AppDbContext na aplicação
// e diz que ele vai usar um banco em memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GlobalBankDb"));

var app = builder.Build();

// Se estiver em desenvolvimento, abre o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Faz a API redirecionar para HTTPS
app.UseHttpsRedirection();

// Liga a autorização
app.UseAuthorization();

// Diz para a aplicação usar as rotas dos controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
