var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // HSTS'yi üretim ortamı için etkinleştiriyoruz.
}

app.UseHttpsRedirection();  // HTTPS'ye yönlendirme

app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();  // Statik dosyalar için kullanılır.

app.MapRazorPages();

app.Run();
