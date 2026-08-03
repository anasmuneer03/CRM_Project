using CRM.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {

        public DbSet<Lead> Leads { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Sale> Sales { get; set; }
        //public DbSet<Invoice> Invoices { get; set; }
        //public DbSet<Payment> Payments { get; set; }
        public DbSet<CrmTask> Tasks { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
            )
        : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //fluent api
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            builder.Entity<Lead>()
                .HasOne(l => l.ConvertedToCustomer)
                .WithOne(c => c.ConvertedFromLead)
                .HasForeignKey<Lead>(l => l.ConvertedToCustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Lead>()
                .HasOne(l => l.AssignedAgent)
                .WithMany(a => a.AssignedLeads)
                .HasForeignKey(l => l.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Customer>()
                .HasOne(c => c.AssignedAgent)
                .WithMany(a => a.AssignedCustomers)
                .HasForeignKey(c => c.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Opportunity>()
                .HasOne(o => o.AssignedAgent)
                .WithMany(a => a.AssignedOpportunities)
                .HasForeignKey(o => o.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Opportunity>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Opportunities)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Opportunity>()
                .HasOne(o => o.Sale)
                .WithOne(s => s.Opportunity)
                .HasForeignKey<Sale>(s => s.OpportunityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(s => s.AssignedAgent)
                .WithMany(a => a.AssignedSales)
                .HasForeignKey(s => s.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Invoice>()
            //    .HasOne(i => i.Sale)
            //    .WithMany(s => s.Invoices)
            //    .HasForeignKey(i => i.SaleId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Invoice>()
            //    .HasIndex(i => i.InvoiceNumber)
            //    .IsUnique();

            //builder.Entity<Payment>()
            //    .HasOne(p => p.Invoice)
            //    .WithMany(i => i.Payments)
            //    .HasForeignKey(p => p.InvoiceId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CrmTask>()
                .HasOne(t => t.AssignedTo)
                .WithMany(a => a.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<CrmTask>()
                .HasOne(t => t.Lead)
                .WithMany(l => l.CrmTasks)
                .HasForeignKey(t => t.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CrmTask>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.CrmTasks)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CrmTask>()
                .HasOne(t => t.Opportunity)
                .WithMany(o => o.CrmTasks)
                .HasForeignKey(t => t.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Activity>()
                .HasOne(a => a.Lead)
                .WithMany(l => l.Activities)
                .HasForeignKey(a => a.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Activity>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Activity>()
                .HasOne(a => a.Opportunity)
                .WithMany(o => o.Activities)
                .HasForeignKey(a => a.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Note>()
                .HasOne(n => n.Lead)
                .WithMany(l => l.Notes)
                .HasForeignKey(n => n.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Note>()
                .HasOne(n => n.Customer)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Note>()
                .HasOne(n => n.Opportunity)
                .WithMany(o => o.Notes)
                .HasForeignKey(n => n.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attachment>()
                .HasOne(a => a.Lead)
                .WithMany(l => l.Attachments)
                .HasForeignKey(a => a.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attachment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Attachments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attachment>()
                .HasOne(a => a.Opportunity)
                .WithMany(o => o.Attachments)
                .HasForeignKey(a => a.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);


        }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    if (_httpContextAccessor.HttpContext != null)
        //    {
        //        var entries = ChangeTracker.Entries<AuditableEntity>();
        //        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        foreach (var entry in entries)
        //        {
        //            if (entry.State == EntityState.Added)
        //            {
        //                entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
        //                entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
        //            }
        //            if (entry.State == EntityState.Modified)
        //            {
        //                entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
        //                entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
        //            }

        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
