using Microsoft.EntityFrameworkCore;

namespace PrsWeb.Models;

public partial class PrsdbContext : DbContext
{
    public PrsdbContext()
    {
    }

    public PrsdbContext(DbContextOptions<PrsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=Anuj-HP\\SQLEXPRESS;Database=prsdb;Integrated Security=True;TrustServerCertificate=True;");

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<LineItem>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__LineItem__3214EC077B20021A");

    //            entity.HasOne(d => d.Product).WithMany(p => p.LineItems)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__LineItem__Produc__38996AB5");

    //            entity.HasOne(d => d.Request).WithMany(p => p.LineItems)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__LineItem__Reques__398D8EEE");
    //        });

    //        modelBuilder.Entity<Product>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC074A1F6656");

    //            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__Product__VendorI__300424B4");
    //        });

    //        modelBuilder.Entity<Request>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC07F5FE978B");

    //            entity.Property(e => e.Status).HasDefaultValue("New");
    //            entity.Property(e => e.SubmittedDate).HasDefaultValueSql("(getdate())");

    //            entity.HasOne(d => d.User).WithMany(p => p.Requests)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__Request__UserId__34C8D9D1");
    //        });

    //        modelBuilder.Entity<User>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__User__3214EC070B54F9DB");
    //        });

    //        modelBuilder.Entity<Vendor>(entity =>
    //        {
    //            entity.HasKey(e => e.Id).HasName("PK__Vendor__3214EC07FDF059A3");
    //        });

    //        OnModelCreatingPartial(modelBuilder);
    //    }

    //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Request>()
    //        .HasOne(r => r.User)
    //        .WithMany(u => u.Requests)
    //        .HasForeignKey(r => r.UserId)
    //        .OnDelete(DeleteBehavior.Cascade);  // Cascade delete
    //}
}
