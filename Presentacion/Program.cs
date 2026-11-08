using Aplicacion.Interface;
using Aplicacion.Services;
using Dominio.Entidades;
using Estructura.database;
using Estructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

var con = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new BD(con!));

// Repositorios
builder.Services.AddScoped<IRol, RolesRepositorio>();
builder.Services.AddScoped<IUsuario, usuarioRepositorio>();
builder.Services.AddScoped<IClientes, ClientesRepositorio>();
builder.Services.AddScoped<IProveedor, ProveedorRepositorio>();
builder.Services.AddScoped<IProductos, ProductosRepositorio>();
builder.Services.AddScoped<ICompra, CompraRepositorio>();
builder.Services.AddScoped<IDetalleCompra, DetalleCompraRepositorio>();
builder.Services.AddScoped<IVenta, VentaRepositorio>();
builder.Services.AddScoped<IDetalleVenta, DetalleVentaRepositorio>();
builder.Services.AddScoped<IDetalleVentaSerie, DetalleVentaSerieRepositorio>();
builder.Services.AddScoped<ICredito, CreditoRepositorio>();
builder.Services.AddScoped<ICuotaCredito, CuotaCreditoRepositorio>();
builder.Services.AddScoped<IAbono, AbonoRepositorio>();
builder.Services.AddScoped<IKardex, KardexRepositorio>();
builder.Services.AddScoped<IReclamoGarantia, ReclamoGarantiaRepositorio>();
builder.Services.AddScoped<IOrdenReparacion, OrdenReparacionRepositorio>();

// Servicios
builder.Services.AddScoped<rolesServices>();
builder.Services.AddScoped<UsuarioServices>();
builder.Services.AddScoped<Clientes_Services>();
builder.Services.AddScoped<ProveedorServices>();
builder.Services.AddScoped<ProductosServices>();
builder.Services.AddScoped<CompraServices>();
builder.Services.AddScoped<DetalleCompraServices>();
builder.Services.AddScoped<VentaServices>();
builder.Services.AddScoped<DetalleVentaServices>();
builder.Services.AddScoped<DetalleVentaSerieServices>();
builder.Services.AddScoped<CreditoServices>();
builder.Services.AddScoped<CuotaCreditoServices>();
builder.Services.AddScoped<AbonoServices>();
builder.Services.AddScoped<KardexServices>();
builder.Services.AddScoped<ReclamoGarantiaServices>();
builder.Services.AddScoped<OrdenReparacionServices>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
