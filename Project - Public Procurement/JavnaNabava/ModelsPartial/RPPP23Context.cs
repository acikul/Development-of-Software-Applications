using Microsoft.EntityFrameworkCore;


namespace JavnaNabava.Models
{
    public partial class RPPP23Context : DbContext
    {
        public virtual DbSet<ViewStrucnjakInfo> vw_Strucnjaci { get; set; }
        public virtual DbSet<ViewPrijavaNaNatjecajInfo> vw_PrijaveNaNatjecaj { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewStrucnjakInfo>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_Strucnjaci");
            });
            modelBuilder.Entity<ViewPrijavaNaNatjecajInfo>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_PrijaveNaNatjecaj");
            });
        }
    }
}
