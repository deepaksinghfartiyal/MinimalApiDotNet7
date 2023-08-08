using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class DbGenricRepositoryPatternContext : DbContext
{
    public DbGenricRepositoryPatternContext()
    {
    }

    public DbGenricRepositoryPatternContext(DbContextOptions<DbGenricRepositoryPatternContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAttribute> BookAttributes { get; set; }

    public virtual DbSet<BookDetail> BookDetails { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-ROD18FU;Database=DB_GenricRepositoryPattern;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3214EC07D23293E7");

            entity.ToTable("Author");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Genre).WithMany(p => p.Authors)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Author__GenreId__778AC167");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC073D278152");

            entity.ToTable("Book");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Book__AuthorId__7A672E12");

            entity.HasOne(d => d.BookDetail).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookDetailId)
                .HasConstraintName("FK__Book__BookDetail__7C4F7684");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Book__PublisherI__7B5B524B");
        });

        modelBuilder.Entity<BookAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookAttr__3214EC07A24B5532");

            entity.ToTable("BookAttribute");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookDeta__3214EC07BCC8E25F");

            entity.ToTable("BookDetail");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC07D0C30A08");

            entity.ToTable("Country");

            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0746406F56");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC073B8B19B6");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("fk_country");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_department");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("fk_job_title");

            entity.HasOne(d => d.Manager).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("fk_manager");

            entity.HasOne(d => d.Project).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("fk_project");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC07BE96BF0B");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.BookAttribute).WithMany(p => p.Genres)
                .HasForeignKey(d => d.BookAttributeId)
                .HasConstraintName("FK__Genre__BookAttri__70DDC3D8");
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobTitle__3214EC07EAA1A685");

            entity.ToTable("JobTitle");

            entity.Property(e => e.JobTitleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC0777DDD5DB");

            entity.ToTable("Manager");

            entity.Property(e => e.ManagerName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC07A4554006");

            entity.ToTable("Project");

            entity.Property(e => e.ProjectName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC072A8FC48B");

            entity.ToTable("Publisher");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__tblEmplo__7AD04F1100C3091F");

            entity.ToTable("tblEmployee");

            entity.Property(e => e.Dept).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
