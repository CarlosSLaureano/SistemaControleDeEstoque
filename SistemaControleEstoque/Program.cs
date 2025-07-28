
using Microsoft.EntityFrameworkCore;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Helper;
using SistemaControleEstoque.Repositorio;

namespace SistemaControleEstoque
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona suporte a user-secrets (útil para dev)
            builder.Configuration.AddUserSecrets<Program>(optional: true);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Corrigido: AddDbContext separado do AddEndpointsApiExplorer
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
            );

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();
            
            builder.Services.AddScoped<IActivityLogger, ActivityLogger>();

            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            // UseSession deve estar antes de UseAuthorization para funcionar corretamente
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
