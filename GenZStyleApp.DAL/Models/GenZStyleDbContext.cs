using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Models
{
    public class GenZStyleDbContext : DbContext 
    {
        public GenZStyleDbContext()
        {

        }
        public GenZStyleDbContext(DbContextOptions<GenZStyleDbContext> opts) : base(opts)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions  { get; set; }
        public DbSet<Wallet> Wallets  { get; set; }
        public DbSet<Notification> Notifications  { get; set; }

        public DbSet<Invoice> Invoices {  get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<InboxPaticipant> InboxPaticipants { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FashionItem> FashionItems { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<StyleFashion> StyleFashions { get; set; }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Account");
                e.Property(e => e.AccountId)
                .ValueGeneratedOnAdd();
                e.Property(acc => acc.Username).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.PasswordHash).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Firstname).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Email).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Lastname).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.IsVip).IsRequired();
                e.Property(acc => acc.IsActive).IsRequired();


                e.HasOne(e => e.User)
                .WithMany(e => e.Accounts)
                .HasForeignKey(e => e.UserId);
                
                e.HasOne(e => e.Inbox)
                .WithOne(e => e.Account)
                .HasForeignKey<Inbox>(e => e.InboxId);

                e.HasOne(e => e.Wallet)
                .WithOne(e => e.Account)
                .HasForeignKey<Wallet>(e => e.WalletId);


            }
            );

            modelBuilder.Entity<Role>(Entity =>
            {
                Entity.ToTable("Role");
                Entity.Property(r => r.Id).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.RoleName).IsUnicode(true).HasMaxLength(50);

            });
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.Property(us => us.UserId)
                .ValueGeneratedOnAdd();             
                e.Property(us => us.City).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Address).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Phone).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Dob).HasColumnType("datetime");
                e.Property(us => us.AvatarUrl).IsUnicode(true).HasMaxLength(int.MaxValue);

                e.HasOne(us => us.Role)
                .WithMany(us => us.Users)
                .HasForeignKey(us => us.RoleId);
                
            }
            );
            modelBuilder.Entity<Invoice>(e =>
            {
                e.ToTable("Invoice");
                e.Property(In => In.InvoiceId)
                .ValueGeneratedOnAdd();
                e.Property(In => In.Total).HasColumnType("decimal(18, 2)").IsRequired();
                e.Property(In => In.PaymentType).IsUnicode(true).HasMaxLength(50);
                e.Property(In => In.Status).IsUnicode(true).HasMaxLength(50);
                e.Property(In => In.Date).HasColumnType("datetime");

                e.HasOne(In => In.Account)
                .WithMany(In => In.Invoices)
                .HasForeignKey(In => In.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                e.HasOne(In => In.Package)
                .WithMany(In => In.Invoices)
                .HasForeignKey(In => In.packageId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            }
            );
            modelBuilder.Entity<Package>(Entity =>
            {
                Entity.ToTable("Package");
                Entity.Property(p => p.PackageId)
                .ValueGeneratedOnAdd();
                Entity.Property(p => p.PackageName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(p => p.Cost).HasColumnType("decimal(18, 2)").IsRequired();
                Entity.Property(p => p.Image).IsUnicode(true).HasMaxLength(50);

            });

            modelBuilder.Entity<Payment>(Entity =>
            {
                Entity.ToTable("Payment");
                Entity.Property(pa => pa.PaymentId)
                .ValueGeneratedOnAdd();
                Entity.Property(pa => pa.PaymentMethod).IsUnicode(true).HasMaxLength(50);
                Entity.Property(pa => pa.Amount).HasColumnType("decimal(18, 2)").IsRequired();
                Entity.Property(pa => pa.PaymentDate).HasColumnType("datetime");

                Entity.HasOne(pa => pa.Invoice)
                .WithMany(pa => pa.Payments)
                .HasForeignKey(pa => pa.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.ToTable("Transaction");
                e.Property(Tr => Tr.Id)
                .ValueGeneratedOnAdd();
                e.Property(us => us.TransStyle).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Amount).HasColumnType("decimal(18, 2)").IsRequired();
                e.Property(us => us.TransactionDate).HasColumnType("datetime");
                

                e.HasOne(us => us.Payment)
                .WithMany(us => us.Transactions)
                .HasForeignKey(us => us.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                e.HasOne(us => us.Payment)
                .WithMany(us => us.Transactions)
                .HasForeignKey(us => us.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            }
            );

            modelBuilder.Entity<Post>(e =>
            {
                e.ToTable("Post");
                e.Property(po => po.PostId)
                .ValueGeneratedOnAdd();
                e.Property(po => po.Content).IsUnicode(true).HasMaxLength(50);
                e.Property(po => po.Image).IsUnicode(true).HasMaxLength(50);
                e.Property(po => po.CreateTime).HasColumnType("datetime");
                e.Property(po => po.UpdateTime).HasColumnType("datetime");



                e.HasOne(po => po.Account)
                .WithMany(po => po.Posts)
                .HasForeignKey(po => po.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            }
            );
            modelBuilder.Entity<Notification>(e =>
            {
                e.ToTable("Notification");
                e.Property(no => no.NotificationId)
                .ValueGeneratedOnAdd();
                e.Property(no => no.Message).IsUnicode(true).HasMaxLength(50);
                e.Property(no => no.CreateAt).HasColumnType("datetime");
                e.Property(no => no.UpdateAt).HasColumnType("datetime");



                e.HasOne(no => no.Account)
                .WithMany(no => no.Notifications)
                .HasForeignKey(no => no.AccountId);


            }
            );
            modelBuilder.Entity<Wallet>(Entity =>
            {
                Entity.ToTable("Wallet");
                Entity.Property(w => w.WalletId)
                .ValueGeneratedOnAdd();
                Entity.Property(w => w.Balance ).HasColumnType("decimal(18, 2)").IsRequired();
                Entity.Property(po => po.CreatDate).HasColumnType("datetime");
                
                

                Entity.HasOne(e => e.Account)
                .WithOne(e => e.Wallet)
                .HasForeignKey<Account>(e => e.AccountId);

            });
            modelBuilder.Entity<Like>(Entity =>
            {   
                Entity.ToTable("Like");
                Entity.HasKey(e => new { e.AccountId, e.PostId });
                

                Entity.HasOne(L => L.Post)
                .WithMany(L => L.Likes)
                .HasForeignKey(po => po.PostId);

                Entity.HasOne(L => L.Account)
                .WithMany(L => L.Likes)
                .HasForeignKey(po => po.AccountId);

            });

            modelBuilder.Entity<Message>(Entity =>
            {
                Entity.ToTable("Message");

                Entity.HasKey(e => new { e.AccountId, e.InboxId });
                Entity.Property(m => m.Content).IsUnicode(true).HasMaxLength(50);
                Entity.Property(m => m.File).IsUnicode(true).HasMaxLength(50);
                Entity.Property(m => m.CreateAt).HasColumnType("datetime");
                Entity.Property(m => m.DeleteAt).HasColumnType("datetime");

                Entity.HasOne(m => m.Account)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);


                Entity.HasOne(m => m.Inbox)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.InboxId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<Inbox>(Entity =>
            {
                Entity.ToTable("Inbox");
                Entity.Property(m => m.InboxId)
                .ValueGeneratedOnAdd();

                Entity.Property(m => m.LastMessage).IsUnicode(true).HasMaxLength(50);

                Entity.HasOne(m => m.Account)
                .WithOne(m => m.Inbox)
                .HasForeignKey<Inbox>(m => m.AccountId);


            });

            modelBuilder.Entity<InboxPaticipant>(Entity =>
            {
                Entity.ToTable("InboxPaticipant");
                Entity.Property(pa => pa.InboxPaticipantId)
                .ValueGeneratedOnAdd();


                Entity.HasOne(pa => pa.Account)
                .WithMany(pa => pa.InboxPaticipants)
                .HasForeignKey(pa => pa.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);


                Entity.HasOne(pa => pa.Inbox)
                .WithMany(pa => pa.Paticipants)
                .HasForeignKey(pa => pa.InboxId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            });

            modelBuilder.Entity<UserRelation>(Entity =>
            {
                Entity.ToTable("UserRelation");
                Entity.HasKey(e => new { e.FollowerId, e.FollowingId });




                Entity.HasOne(u => u.Account)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(u => u.FollowerId);

                Entity.HasOne(u => u.Account)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(u => u.FollowingId);
            });

            modelBuilder.Entity<FashionItem>(Entity =>
            {
                Entity.ToTable("FashionItem");
                Entity.Property(fa => fa.FashionId)
                .ValueGeneratedOnAdd();
                Entity.Property(fa => fa.fashionName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(fa => fa.fashionDescription).IsUnicode(true).HasMaxLength(50);
                Entity.Property(fa => fa.Image).IsUnicode(true).HasMaxLength(50);
                Entity.Property(fa => fa.Cost).HasColumnType("decimal(18, 2)").IsRequired();

                Entity.HasOne(fa => fa.Category)
                .WithMany(fa => fa.FashionItems)
                .HasForeignKey(fa => fa.CategoryId);

                Entity.HasOne(fa => fa.Post)
                .WithMany(fa => fa.FashionItems)
                .HasForeignKey(fa => fa.PostId);

            });
            modelBuilder.Entity<Category>(Entity =>
            {
                Entity.ToTable("Category");
                Entity.Property(fa => fa.CategoriesId)
                .ValueGeneratedOnAdd();
                Entity.Property(fa => fa.CategoryName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(fa => fa.CategoryDescription).IsUnicode(true).HasMaxLength(50);
                

                
            });
            modelBuilder.Entity<Comment>(Entity =>
            {
                Entity.ToTable("Comment");
                Entity.Property(cm => cm.CommentId)
                .ValueGeneratedOnAdd();
                Entity.Property(cm => cm.Content).IsUnicode(true).HasMaxLength(50);
                Entity.Property(cm => cm.CreateAt).HasColumnType("datetime");


                Entity.HasOne(cm => cm.ParentComment)
                .WithMany(cm => cm.SubComments)
                .HasForeignKey(cm => cm.ParentCommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);


                Entity.HasOne(cm => cm.Post) 
                .WithMany(cm => cm.Comments)
                .HasForeignKey(cm => cm.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                Entity.HasOne(cm => cm.Account)
                .WithMany(cm => cm.Comments)
                .HasForeignKey(cm => cm.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            });

            modelBuilder.Entity<Style>(Entity =>
            {
                Entity.ToTable("Style");
                Entity.Property(st => st.StyleId)
                .ValueGeneratedOnAdd();
                Entity.Property(st => st.StyleName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(st => st.Description).IsUnicode(true).HasMaxLength(50);
                Entity.Property(st => st.CreateAt).HasColumnType("datetime");
                Entity.Property(st => st.UpdateAt).HasColumnType("datetime");

                Entity.HasOne(st => st.Account )
                .WithMany(st => st.Styles)
                .HasForeignKey(st => st.AccountId);



            });
            modelBuilder.Entity<StyleFashion>(Entity =>
            {
                Entity.ToTable("StyleFashion");
                Entity.HasKey(sf => new {sf.StyleId ,sf.FashionId  });
                
                Entity.Property(sf => sf.CreateAt).HasColumnType("datetime");
                Entity.Property(sf => sf.UpdateAt).HasColumnType("datetime");

                modelBuilder.Entity<StyleFashion>()
                .HasOne(it => it.FashionItem)
                .WithMany(a => a.StyleFashions)
                .HasForeignKey(it => it.FashionId); 

                modelBuilder.Entity<StyleFashion>()
                    .HasOne(it => it.Style)
                    .WithMany(b => b.StyleFashions)
                    .HasForeignKey(it => it.StyleId);

                

            });




        }

    }
}
