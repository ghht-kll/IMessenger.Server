using IMessenger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace IMessenger.Server.Src
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=IMessengerDb;Username=postgres;Password=1956");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Receiverid).HasColumnName("receiverid");

                entity.Property(e => e.Senderid).HasColumnName("senderid");

                entity.Property(e => e.Sendingtime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("sendingtime");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasPrincipalKey(p => p.Userid)
                    .HasForeignKey(d => d.Receiverid)
                    .HasConstraintName("messages_receiverid_fkey");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasPrincipalKey(p => p.Userid)
                    .HasForeignKey(d => d.Senderid)
                    .HasConstraintName("messages_senderid_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Login, "users_login_key")
                    .IsUnique();

                entity.HasIndex(e => e.Password, "users_password_key")
                    .IsUnique();

                entity.HasIndex(e => e.Userid, "users_userid_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.About).HasColumnName("about");

                entity.Property(e => e.Login).HasColumnName("login");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Photouri).HasColumnName("photouri");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Username).HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
