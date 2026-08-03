var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Swagger kurulum kodları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger arayüzünü tarayıcıda göstermek için gerekli izinler
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Controller adreslerini (bizim api/v1/chat) aktif eder
app.MapControllers();

app.Run();