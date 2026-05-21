using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);
        ConfigureProducts(modelBuilder);
        ConfigureCarts(modelBuilder);
        ConfigureCartItems(modelBuilder);
        ConfigureOrders(modelBuilder);
        ConfigureOrderItems(modelBuilder);
        SeedProducts(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Names).HasMaxLength(120).IsRequired();
            entity.Property(user => user.LastNames).HasMaxLength(120).IsRequired();
            entity.Property(user => user.Country).HasMaxLength(80).IsRequired();
            entity.Property(user => user.Department).HasMaxLength(80).IsRequired();
            entity.Property(user => user.City).HasMaxLength(80).IsRequired();
            entity.Property(user => user.Phone).HasMaxLength(30).IsRequired();
            entity.Property(user => user.Address).HasMaxLength(180).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(180).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(30).IsRequired();

            entity.HasIndex(user => user.Email).IsUnique();
        });
    }

    private static void ConfigureProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);

            entity.Property(product => product.Code).HasMaxLength(40).IsRequired();
            entity.Property(product => product.ImageUrl).HasMaxLength(500).IsRequired();
            entity.Property(product => product.Name).HasMaxLength(160).IsRequired();
            entity.Property(product => product.Description).HasMaxLength(1000).IsRequired();
            entity.Property(product => product.Color).HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(product => product.Price).HasPrecision(18, 2).IsRequired();
            entity.Property(product => product.Stock).IsRequired();

            entity.HasIndex(product => product.Code).IsUnique();

            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_Products_Price_Positive", "\"Price\" > 0");
                table.HasCheckConstraint("CK_Products_Stock_NonNegative", "\"Stock\" >= 0");
            });
        });
    }

    private static void ConfigureCarts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(cart => cart.Id);

            entity.HasOne(cart => cart.User)
                .WithOne(user => user.Cart)
                .HasForeignKey<Cart>(cart => cart.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(cart => cart.UserId).IsUnique();
        });
    }

    private static void ConfigureCartItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Quantity).IsRequired();

            entity.HasOne(item => item.Cart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(item => item.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Product)
                .WithMany(product => product.CartItems)
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(item => new { item.CartId, item.ProductId }).IsUnique();

            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_CartItems_Quantity_Positive", "\"Quantity\" > 0");
            });
        });
    }

    private static void ConfigureOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.Id);

            entity.Property(order => order.Total).HasPrecision(18, 2).IsRequired();
            entity.Property(order => order.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(order => order.CreatedAt).IsRequired();

            entity.HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_Orders_Total_NonNegative", "\"Total\" >= 0");
            });
        });
    }

    private static void ConfigureOrderItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(item => item.Id);

            entity.Property(item => item.Price).HasPrecision(18, 2).IsRequired();
            entity.Property(item => item.Quantity).IsRequired();

            entity.HasOne(item => item.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_OrderItems_Price_Positive", "\"Price\" > 0");
                table.HasCheckConstraint("CK_OrderItems_Quantity_Positive", "\"Quantity\" > 0");
            });
        });
    }

    private static void SeedProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Code = "SNK-WHT-07-001",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+7",
                Name = "Urban Runner White 7",
                Description = "Tenis urbanos blancos para uso diario.",
                Size = ProductSize.Seven,
                Color = ProductColor.White,
                Price = 179900m,
                Stock = 15
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Code = "SNK-BLK-07-002",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+7",
                Name = "Street Flex Black 7",
                Description = "Tenis negros flexibles con suela liviana.",
                Size = ProductSize.Seven,
                Color = ProductColor.Black,
                Price = 189900m,
                Stock = 12
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Code = "SNK-GRY-08-003",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+8",
                Name = "Daily Move Gray 8",
                Description = "Tenis grises comodos para caminatas largas.",
                Size = ProductSize.Eight,
                Color = ProductColor.Gray,
                Price = 169900m,
                Stock = 20
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Code = "SNK-WHT-08-004",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+8",
                Name = "Clean Step White 8",
                Description = "Tenis blancos minimalistas con acabado suave.",
                Size = ProductSize.Eight,
                Color = ProductColor.White,
                Price = 199900m,
                Stock = 10
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Code = "SNK-BLK-08-005",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+8",
                Name = "Night Walk Black 8",
                Description = "Tenis negros casuales resistentes al uso frecuente.",
                Size = ProductSize.Eight,
                Color = ProductColor.Black,
                Price = 209900m,
                Stock = 8
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                Code = "SNK-GRY-09-006",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+9",
                Name = "Metro Pace Gray 9",
                Description = "Tenis grises de estilo deportivo urbano.",
                Size = ProductSize.Nine,
                Color = ProductColor.Gray,
                Price = 219900m,
                Stock = 18
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                Code = "SNK-WHT-09-007",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+9",
                Name = "Air Street White 9",
                Description = "Tenis blancos ligeros con plantilla acolchada.",
                Size = ProductSize.Nine,
                Color = ProductColor.White,
                Price = 229900m,
                Stock = 14
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                Code = "SNK-BLK-10-008",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+10",
                Name = "Core Black 10",
                Description = "Tenis negros clasicos con diseño versatil.",
                Size = ProductSize.Ten,
                Color = ProductColor.Black,
                Price = 239900m,
                Stock = 11
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000009"),
                Code = "SNK-GRY-10-009",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+10",
                Name = "Balance Gray 10",
                Description = "Tenis grises con diseño sobrio para uso diario.",
                Size = ProductSize.Ten,
                Color = ProductColor.Gray,
                Price = 189900m,
                Stock = 16
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000010"),
                Code = "SNK-WHT-10-010",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+10",
                Name = "Classic White 10",
                Description = "Tenis blancos clasicos para combinaciones casuales.",
                Size = ProductSize.Ten,
                Color = ProductColor.White,
                Price = 159900m,
                Stock = 22
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000011"),
                Code = "SNK-GRY-07-011",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+7",
                Name = "Soft Track Gray 7",
                Description = "Tenis grises livianos para jornadas activas.",
                Size = ProductSize.Seven,
                Color = ProductColor.Gray,
                Price = 174900m,
                Stock = 13
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000012"),
                Code = "SNK-WHT-07-012",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+7+Alt",
                Name = "Fresh Walk White 7",
                Description = "Tenis blancos con perfil bajo y suela flexible.",
                Size = ProductSize.Seven,
                Color = ProductColor.White,
                Price = 184900m,
                Stock = 9
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000013"),
                Code = "SNK-BLK-08-013",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+8+Alt",
                Name = "Shadow Fit Black 8",
                Description = "Tenis negros de ajuste comodo para uso urbano.",
                Size = ProductSize.Eight,
                Color = ProductColor.Black,
                Price = 214900m,
                Stock = 17
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000014"),
                Code = "SNK-GRY-08-014",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+8+Alt",
                Name = "Cloud Step Gray 8",
                Description = "Tenis grises con amortiguacion para caminar.",
                Size = ProductSize.Eight,
                Color = ProductColor.Gray,
                Price = 204900m,
                Stock = 19
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000015"),
                Code = "SNK-WHT-09-015",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+9+Alt",
                Name = "Minimal White 9",
                Description = "Tenis blancos minimalistas para combinaciones limpias.",
                Size = ProductSize.Nine,
                Color = ProductColor.White,
                Price = 194900m,
                Stock = 21
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000016"),
                Code = "SNK-BLK-09-016",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+9",
                Name = "Urban Core Black 9",
                Description = "Tenis negros con acabado mate y suela resistente.",
                Size = ProductSize.Nine,
                Color = ProductColor.Black,
                Price = 224900m,
                Stock = 12
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000017"),
                Code = "SNK-GRY-09-017",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+9+Alt",
                Name = "Route Gray 9",
                Description = "Tenis grises versatiles para salidas casuales.",
                Size = ProductSize.Nine,
                Color = ProductColor.Gray,
                Price = 199900m,
                Stock = 15
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000018"),
                Code = "SNK-WHT-10-018",
                ImageUrl = "https://placehold.co/600x400?text=White+Sneaker+10+Alt",
                Name = "Pure Motion White 10",
                Description = "Tenis blancos con plantilla suave y estilo casual.",
                Size = ProductSize.Ten,
                Color = ProductColor.White,
                Price = 234900m,
                Stock = 10
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000019"),
                Code = "SNK-BLK-10-019",
                ImageUrl = "https://placehold.co/600x400?text=Black+Sneaker+10+Alt",
                Name = "Bold Step Black 10",
                Description = "Tenis negros de corte clasico para uso diario.",
                Size = ProductSize.Ten,
                Color = ProductColor.Black,
                Price = 244900m,
                Stock = 7
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000020"),
                Code = "SNK-GRY-10-020",
                ImageUrl = "https://placehold.co/600x400?text=Gray+Sneaker+10+Alt",
                Name = "City Flow Gray 10",
                Description = "Tenis grises con estructura estable y suela liviana.",
                Size = ProductSize.Ten,
                Color = ProductColor.Gray,
                Price = 229900m,
                Stock = 18
            });
    }
}
