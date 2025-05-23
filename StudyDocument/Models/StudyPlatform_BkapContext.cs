using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudyDocument.Models;

public partial class StudyPlatform_BkapContext : DbContext
{
    public StudyPlatform_BkapContext()
    {
    }

    public StudyPlatform_BkapContext(DbContextOptions<StudyPlatform_BkapContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Advertise> Advertises { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Config> Configs { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Viewer> Viewers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=GIANGNGUYEN;Database=StudyPlatform_BKAP;uid=sa;pwd=123456;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC07D2C29C3D");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserName, "UQ__Admin__C9F284566E8870DC").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Avatar)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(300);
            entity.Property(e => e.IdCard)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(300)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfIssue).HasMaxLength(200);
            entity.Property(e => e.Role).HasDefaultValue(1);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UserName)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Advertise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Advertis__3214EC073D644317");

            entity.ToTable("Advertise");

            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Start).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC074C7097C9");

            entity.ToTable("Category");

            entity.Property(e => e.Descrtiption).HasMaxLength(300);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Keyword).HasMaxLength(300);
            entity.Property(e => e.Lang).HasMaxLength(20);
            entity.Property(e => e.Level).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Tag).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(300);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07C0B64305");

            entity.ToTable("Comment");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DocumentId).HasColumnName("Document_id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ImageId).HasColumnName("Image_id");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.VideoId).HasColumnName("Video_id");

            entity.HasOne(d => d.Document).WithMany(p => p.Comments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Documen__4E88ABD4");

            entity.HasOne(d => d.Image).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Image_i__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__User_id__4BAC3F29");

            entity.HasOne(d => d.Video).WithMany(p => p.Comments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Video_i__4D94879B");
        });

        modelBuilder.Entity<Config>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Config__3214EC07DC0C964B");

            entity.ToTable("Config");

            entity.Property(e => e.Contact)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Coppyright)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.GoogleId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Hotline)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Keyword)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Lang)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LinkVideo1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LinkVideo2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LinkVideo3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LinkVideo4)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MailInfo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Mail_Info");
            entity.Property(e => e.MailNoreply)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Mail_noreply");
            entity.Property(e => e.MailPassword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Mail_Password");
            entity.Property(e => e.MailPort)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Mail_Port");
            entity.Property(e => e.PlaceBody)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PlaceHead)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink4)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink5)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SocialLink6)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contact__3214EC072DA0CE27");

            entity.ToTable("Contact");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.Phone).HasMaxLength(300);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07303229A3");

            entity.ToTable("Document");

            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.File).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Document__Catego__48CFD27E");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC07F37BD928");

            entity.ToTable("Image");

            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.Image1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Image");
            entity.Property(e => e.ImageType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Images)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Image__Category___45F365D3");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC07E441A5C8");

            entity.ToTable("Menu");

            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Detail).HasColumnType("ntext");
            entity.Property(e => e.Keyword).HasMaxLength(300);
            entity.Property(e => e.Lang)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Level)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Tag).HasMaxLength(300);
            entity.Property(e => e.Target)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0740A0FE62");

            entity.ToTable("User");

            entity.HasIndex(e => e.UserName, "UQ__User__C9F28456A6B0C6B2").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Avatar)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(300);
            entity.Property(e => e.IdCard)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Image1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Image2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Image3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(300)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfIssue).HasMaxLength(200);
            entity.Property(e => e.UserName)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Video__3214EC0719E29B9E");

            entity.ToTable("Video");

            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("Create_time");
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Video1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Video");
            entity.Property(e => e.VideoType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Videos)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Video__Category___4316F928");
        });

        modelBuilder.Entity<Viewer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Viewer__3214EC0783D8EF6B");

            entity.ToTable("Viewer");

            entity.Property(e => e.AccessTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.LocalIp).HasMaxLength(300);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.PublicIp).HasMaxLength(300);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TotalAccessCountMonth).HasDefaultValue(0);
            entity.Property(e => e.TotalAccessCountWeek).HasDefaultValue(0);
            entity.Property(e => e.TotalAccessCountYear).HasDefaultValue(0);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    internal object Include(Func<object, object> value)
    {
        throw new NotImplementedException();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
