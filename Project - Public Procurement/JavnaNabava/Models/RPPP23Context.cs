using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class RPPP23Context : DbContext
    {
        public RPPP23Context()
        {
        }

        public RPPP23Context(DbContextOptions<RPPP23Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Cpv> Cpvs { get; set; }
        public virtual DbSet<Drzava> Drzavas { get; set; }
        public virtual DbSet<Godina> Godinas { get; set; }
        public virtual DbSet<Grad> Grads { get; set; }
        public virtual DbSet<Kompetencija> Kompetencijas { get; set; }
        public virtual DbSet<KontaktNarucitelj> KontaktNaruciteljs { get; set; }
        public virtual DbSet<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
        public virtual DbSet<Konzorcij> Konzorcijs { get; set; }
        public virtual DbSet<Kriterij> Kriterijs { get; set; }
        public virtual DbSet<Narucitelj> Naruciteljs { get; set; }
        public virtual DbSet<Natjecaj> Natjecajs { get; set; }
        public virtual DbSet<NatjecajZakon> NatjecajZakons { get; set; }
        public virtual DbSet<Ovlastenik> Ovlasteniks { get; set; }
        public virtual DbSet<PlanNabave> PlanNabaves { get; set; }
        public virtual DbSet<Ponuditelj> Ponuditeljs { get; set; }
        public virtual DbSet<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
        public virtual DbSet<SluzbeniDokument> SluzbeniDokuments { get; set; }
        public virtual DbSet<StatusPrijave> StatusPrijaves { get; set; }
        public virtual DbSet<StatusStavke> StatusStavkes { get; set; }
        public virtual DbSet<Stavka> Stavkas { get; set; }
        public virtual DbSet<StavkaPlanaNabave> StavkaPlanaNabaves { get; set; }
        public virtual DbSet<StrucnaSprema> StrucnaSpremas { get; set; }
        public virtual DbSet<Strucnjak> Strucnjaks { get; set; }
        public virtual DbSet<Troskovnik> Troskovniks { get; set; }
        public virtual DbSet<Valutum> Valuta { get; set; }
        public virtual DbSet<VrijednostiKriterija> VrijednostiKriterijas { get; set; }
        public virtual DbSet<VrstaDokumentum> VrstaDokumenta { get; set; }
        public virtual DbSet<VrstaKompetencije> VrstaKompetencijes { get; set; }
        public virtual DbSet<VrstaKontaka> VrstaKontakas { get; set; }
        public virtual DbSet<VrstaStavke> VrstaStavkes { get; set; }
        public virtual DbSet<Zakon> Zakons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<Cpv>(entity =>
            {
                entity.HasKey(e => e.SifCpv)
                    .HasName("PK__CPV__699150B66770FDE2");

                entity.ToTable("CPV");

                entity.Property(e => e.SifCpv)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sifCPV");

                entity.Property(e => e.OpisCpv)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("opisCPV");
            });

            modelBuilder.Entity<Drzava>(entity =>
            {
                entity.HasKey(e => e.SifDrzave)
                    .HasName("PK__Drzava__616A54589B3CF729");

                entity.ToTable("Drzava");

                entity.Property(e => e.SifDrzave).HasColumnName("sifDrzave");

                entity.Property(e => e.NazivDrzava)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nazivDrzava");
            });

            modelBuilder.Entity<Godina>(entity =>
            {
                entity.HasKey(e => e.SifGodine)
                    .HasName("PK__Godina__25F933759519F99E");

                entity.ToTable("Godina");

                entity.Property(e => e.SifGodine).HasColumnName("sifGodine");

                entity.Property(e => e.VrijednostGodine).HasColumnName("vrijednostGodine");
            });

            modelBuilder.Entity<Grad>(entity =>
            {
                entity.HasKey(e => e.SifGrada)
                    .HasName("PK__Grad__532507A345A341F3");

                entity.ToTable("Grad");

                entity.Property(e => e.SifGrada).HasColumnName("sifGrada");

                entity.Property(e => e.NazivGrada)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivGrada");

                entity.Property(e => e.SifDrzava).HasColumnName("sifDrzava");

                entity.HasOne(d => d.SifDrzavaNavigation)
                    .WithMany(p => p.Grads)
                    .HasForeignKey(d => d.SifDrzava)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grad__sifDrzava__300424B4");
            });

            modelBuilder.Entity<Kompetencija>(entity =>
            {
                entity.HasKey(e => e.SifKompetencije)
                    .HasName("PK__Kompeten__E41949633E02D1FC");

                entity.ToTable("Kompetencija");

                entity.Property(e => e.SifKompetencije).HasColumnName("sifKompetencije");

                entity.Property(e => e.DetaljiKompetencije)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("detaljiKompetencije");

                entity.Property(e => e.Oibstrucnjaka)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBstrucnjaka");

                entity.Property(e => e.SifVrsteKompetencije).HasColumnName("sifVrsteKompetencije");

                entity.HasOne(d => d.OibstrucnjakaNavigation)
                    .WithMany(p => p.Kompetencijes)
                    .HasForeignKey(d => d.Oibstrucnjaka)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kompetenc__OIBst__4BAC3F29");

                entity.HasOne(d => d.SifVrsteKompetencijeNavigation)
                    .WithMany(p => p.Kompetencijes)
                    .HasForeignKey(d => d.SifVrsteKompetencije)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kompetenc__sifVr__4CA06362");
            });

            modelBuilder.Entity<KontaktNarucitelj>(entity =>
            {
                entity.HasKey(e => e.SifKontakt)
                    .HasName("PK__KontaktN__C561A73AED8A7225");

                entity.ToTable("KontaktNarucitelj");

                entity.Property(e => e.SifKontakt).HasColumnName("sifKontakt");

                entity.Property(e => e.Oibnarucitelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBNarucitelja");

                entity.Property(e => e.SifVrsteKontakta).HasColumnName("sifVrsteKontakta");

                entity.Property(e => e.TekstKontakta)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("tekstKontakta");

                entity.HasOne(d => d.OibnaruciteljaNavigation)
                    .WithMany(p => p.KontaktNaruciteljs)
                    .HasForeignKey(d => d.Oibnarucitelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KontaktNa__OIBNa__398D8EEE");

                entity.HasOne(d => d.SifVrsteKontaktaNavigation)
                    .WithMany(p => p.KontaktNaruciteljs)
                    .HasForeignKey(d => d.SifVrsteKontakta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KontaktNa__sifVr__38996AB5");
            });

            modelBuilder.Entity<KontaktPonuditelj>(entity =>
            {
                entity.HasKey(e => e.SifKontakt)
                    .HasName("PK__KontaktP__C561A73A7D499D75");

                entity.ToTable("KontaktPonuditelj");

                entity.Property(e => e.SifKontakt).HasColumnName("sifKontakt");

                entity.Property(e => e.Oibponuditelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBPonuditelja");

                entity.Property(e => e.SifVrsteKontakta).HasColumnName("sifVrsteKontakta");

                entity.Property(e => e.TekstKontakta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tekstKontakta");

                entity.HasOne(d => d.OibponuditeljaNavigation)
                    .WithMany(p => p.KontaktPonuditeljs)
                    .HasForeignKey(d => d.Oibponuditelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KontaktPo__OIBPo__4316F928");

                entity.HasOne(d => d.SifVrsteKontaktaNavigation)
                    .WithMany(p => p.KontaktPonuditeljs)
                    .HasForeignKey(d => d.SifVrsteKontakta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KontaktPo__sifVr__440B1D61");
            });

            modelBuilder.Entity<Konzorcij>(entity =>
            {
                entity.HasKey(e => e.Oibkonzorcija)
                    .HasName("PK__Konzorci__C2CE7BDD932C4B3F");

                entity.ToTable("Konzorcij");

                entity.Property(e => e.Oibkonzorcija)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBKonzorcija");

                entity.Property(e => e.NazivKonzorcija)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivKonzorcija");

                entity.Property(e => e.SifDrzave).HasColumnName("sifDrzave");

                entity.HasOne(d => d.SifDrzaveNavigation)
                    .WithMany(p => p.Konzorcijs)
                    .HasForeignKey(d => d.SifDrzave)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Konzorcij__sifDr__32E0915F");
            });

            modelBuilder.Entity<Kriterij>(entity =>
            {
                entity.HasKey(e => e.SifKriterija)
                    .HasName("PK__Kriterij__96106DECFDEFC084");

                entity.ToTable("Kriterij");

                entity.Property(e => e.SifKriterija).HasColumnName("sifKriterija");

                entity.Property(e => e.NazivKriterija)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivKriterija");
            });

            modelBuilder.Entity<Narucitelj>(entity =>
            {
                entity.HasKey(e => e.Oibnarucitelja)
                    .HasName("PK__Narucite__0D2003DA0D510BCB");

                entity.ToTable("Narucitelj");

                entity.Property(e => e.Oibnarucitelja)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBNarucitelja");

                entity.Property(e => e.AdresaNarucitelja)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("adresaNarucitelja");

                entity.Property(e => e.KlasaNarucitelja)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("klasaNarucitelja");

                entity.Property(e => e.NazivNarucitelja)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivNarucitelja");

                entity.Property(e => e.PbrNarucitelja).HasColumnName("pbrNarucitelja");

                entity.Property(e => e.SifGrada).HasColumnName("sifGrada");

                entity.HasOne(d => d.SifGradaNavigation)
                    .WithMany(p => p.Naruciteljs)
                    .HasForeignKey(d => d.SifGrada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Narucitel__sifGr__35BCFE0A");
            });

            modelBuilder.Entity<Natjecaj>(entity =>
            {
                entity.HasKey(e => e.SifNatjecaja)
                    .HasName("PK__Natjecaj__FBB4CDD287DA9D20");

                entity.ToTable("Natjecaj");

                entity.Property(e => e.SifNatjecaja).HasColumnName("sifNatjecaja");

                entity.Property(e => e.DatumObjave)
                    .HasColumnType("date")
                    .HasColumnName("datumObjave");

                entity.Property(e => e.ElektronickaPredaja).HasColumnName("elektronickaPredaja");

                entity.Property(e => e.NazivNatjecaja)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivNatjecaja");

                entity.Property(e => e.Oibnarucitelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBNarucitelja");

                entity.Property(e => e.RokPredaje).HasColumnName("rokPredaje");

                entity.Property(e => e.SifCpv)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sifCPV");

                entity.Property(e => e.SifStavke).HasColumnName("sifStavke");

                entity.Property(e => e.SifTroskovnika).HasColumnName("sifTroskovnika");

                entity.Property(e => e.StatusNatjecaja)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("statusNatjecaja");

                entity.HasOne(d => d.OibnaruciteljaNavigation)
                    .WithMany(p => p.Natjecajs)
                    .HasForeignKey(d => d.Oibnarucitelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Natjecaj__OIBNar__59FA5E80");

                entity.HasOne(d => d.SifCpvNavigation)
                    .WithMany(p => p.Natjecajs)
                    .HasForeignKey(d => d.SifCpv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Natjecaj__sifCPV__5812160E");

                entity.HasOne(d => d.SifStavkeNavigation)
                    .WithMany(p => p.Natjecajs)
                    .HasForeignKey(d => d.SifStavke)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Natjecaj__sifSta__59063A47");

                entity.HasOne(d => d.SifTroskovnikaNavigation)
                    .WithMany(p => p.Natjecajs)
                    .HasForeignKey(d => d.SifTroskovnika)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Natjecaj__sifTro__571DF1D5");
            });

            modelBuilder.Entity<NatjecajZakon>(entity =>
            {
                entity.HasKey(e => new { e.SifNatjecaj, e.SifZakon })
                    .HasName("PK__Natjecaj__729DBC66037F8243");

                entity.ToTable("NatjecajZakon");

                entity.Property(e => e.SifNatjecaj).HasColumnName("sifNatjecaj");

                entity.Property(e => e.SifZakon).HasColumnName("sifZakon");

                entity.HasOne(d => d.SifNatjecajNavigation)
                    .WithMany(p => p.NatjecajZakons)
                    .HasForeignKey(d => d.SifNatjecaj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NatjecajZ__sifNa__6754599E");

                entity.HasOne(d => d.SifZakonNavigation)
                    .WithMany(p => p.NatjecajZakons)
                    .HasForeignKey(d => d.SifZakon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NatjecajZ__sifZa__68487DD7");
            });

            modelBuilder.Entity<Ovlastenik>(entity =>
            {
                entity.HasKey(e => e.Oibovlastenika)
                    .HasName("PK__Ovlasten__87CF8D2D82CA3F28");

                entity.ToTable("Ovlastenik");

                entity.Property(e => e.Oibovlastenika)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBovlastenika");

                entity.Property(e => e.FunkcijaOvlastenika)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("funkcijaOvlastenika");

                entity.Property(e => e.ImeOvlastenika)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("imeOvlastenika");

                entity.Property(e => e.Oibnarucitelj)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBnarucitelj");

                entity.Property(e => e.PrezimeOvlastenika)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("prezimeOvlastenika");

                entity.HasOne(d => d.OibnaruciteljNavigation)
                    .WithMany(p => p.Ovlasteniks)
                    .HasForeignKey(d => d.Oibnarucitelj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ovlasteni__OIBna__3C69FB99");
            });

            modelBuilder.Entity<PlanNabave>(entity =>
            {
                entity.HasKey(e => e.SifPlanaNabave)
                    .HasName("PK__PlanNaba__0644EC439A493434");

                entity.ToTable("PlanNabave");

                entity.Property(e => e.SifPlanaNabave).HasColumnName("sifPlanaNabave");

                entity.Property(e => e.Oibnarucitelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBNarucitelja");

                entity.Property(e => e.SifGodine).HasColumnName("sifGodine");

                entity.HasOne(d => d.OibnaruciteljaNavigation)
                    .WithMany(p => p.PlanNabaves)
                    .HasForeignKey(d => d.Oibnarucitelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PlanNabav__OIBNa__5070F446");

                entity.HasOne(d => d.SifGodineNavigation)
                    .WithMany(p => p.PlanNabaves)
                    .HasForeignKey(d => d.SifGodine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PlanNabav__sifGo__4F7CD00D");
            });

            modelBuilder.Entity<Ponuditelj>(entity =>
            {
                entity.HasKey(e => e.Oibponuditelja)
                    .HasName("PK__Ponudite__7C2D0C666891A6F4");

                entity.ToTable("Ponuditelj");

                entity.Property(e => e.Oibponuditelja)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBPonuditelja");

                entity.Property(e => e.AdresaPonuditelja)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("adresaPonuditelja");

                entity.Property(e => e.KlasaPonuditelja)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("klasaPonuditelja");

                entity.Property(e => e.NazivPonuditelja)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivPonuditelja");

                entity.Property(e => e.Oibkonzorcija)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBKonzorcija");

                entity.Property(e => e.PbrPonuditelja).HasColumnName("pbrPonuditelja");

                entity.Property(e => e.SifGrada).HasColumnName("sifGrada");

                entity.HasOne(d => d.OibkonzorcijaNavigation)
                    .WithMany(p => p.Ponuditeljs)
                    .HasForeignKey(d => d.Oibkonzorcija)
                    .HasConstraintName("FK__Ponuditel__OIBKo__403A8C7D");

                entity.HasOne(d => d.SifGradaNavigation)
                    .WithMany(p => p.Ponuditeljs)
                    .HasForeignKey(d => d.SifGrada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ponuditel__sifGr__3F466844");
            });

            modelBuilder.Entity<PrijavaNaNatjecaj>(entity =>
            {
                entity.HasKey(e => e.SifPrijava)
                    .HasName("PK__Prijava___DD18E75846A54F7D");

                entity.ToTable("Prijava_na_natjecaj");

                entity.Property(e => e.SifPrijava).HasColumnName("sifPrijava");

                entity.Property(e => e.DatumPrijave)
                    .HasColumnType("date")
                    .HasColumnName("datumPrijave");

                entity.Property(e => e.Oibkonzorcija)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBkonzorcija");

                entity.Property(e => e.Oibponuditelja)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBPonuditelja");

                entity.Property(e => e.SifNatjecaja).HasColumnName("sifNatjecaja");

                entity.Property(e => e.SifStatusa).HasColumnName("sifStatusa");

                entity.Property(e => e.SifTroskovnika).HasColumnName("sifTroskovnika");

                entity.Property(e => e.VrstaPonuditelja)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vrstaPonuditelja");

                entity.HasOne(d => d.OibkonzorcijaNavigation)
                    .WithMany(p => p.PrijavaNaNatjecajs)
                    .HasForeignKey(d => d.Oibkonzorcija)
                    .HasConstraintName("FK_Prijava_na_natjecaj_Konzorcij");

                entity.HasOne(d => d.OibponuditeljaNavigation)
                    .WithMany(p => p.PrijavaNaNatjecajs)
                    .HasForeignKey(d => d.Oibponuditelja)
                    .HasConstraintName("FK__Prijava_n__OIBPo__5EBF139D");

                entity.HasOne(d => d.SifNatjecajaNavigation)
                    .WithMany(p => p.PrijavaNaNatjecajs)
                    .HasForeignKey(d => d.SifNatjecaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prijava_n__sifNa__5CD6CB2B");

                entity.HasOne(d => d.SifStatusaNavigation)
                    .WithMany(p => p.PrijavaNaNatjecajs)
                    .HasForeignKey(d => d.SifStatusa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prijava_n__sifSt__5FB337D6");

                entity.HasOne(d => d.SifTroskovnikaNavigation)
                    .WithMany(p => p.PrijavaNaNatjecajs)
                    .HasForeignKey(d => d.SifTroskovnika)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Prijava_n__sifTr__5DCAEF64");
            });

            modelBuilder.Entity<SluzbeniDokument>(entity =>
            {
                entity.HasKey(e => e.SifDokumenta)
                    .HasName("PK__Sluzbeni__E652FF85E5F1152D");

                entity.ToTable("SluzbeniDokument");

                entity.Property(e => e.SifDokumenta).HasColumnName("sifDokumenta");

                entity.Property(e => e.ImeDokumenta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("imeDokumenta");

                entity.Property(e => e.KlasaDokumenta)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("klasaDokumenta");

                entity.Property(e => e.SifPrijave).HasColumnName("sifPrijave");

                entity.Property(e => e.SifVrsteDokumenta).HasColumnName("sifVrsteDokumenta");

                entity.Property(e => e.TekstDokumenta)
                                  .HasColumnName("tekstDokumenta");

                entity.Property(e => e.UrudzbeniBroj).HasColumnName("urudzbeniBroj");

                entity.HasOne(d => d.SifPrijaveNavigation)
                    .WithMany(p => p.SluzbeniDokuments)
                    .HasForeignKey(d => d.SifPrijave)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SluzbeniD__sifPr__6C190EBB");

                entity.HasOne(d => d.SifVrsteDokumentaNavigation)
                    .WithMany(p => p.SluzbeniDokuments)
                    .HasForeignKey(d => d.SifVrsteDokumenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SluzbeniD__sifVr__6B24EA82");
            });

            modelBuilder.Entity<StatusPrijave>(entity =>
            {
                entity.HasKey(e => e.SifStatusa)
                    .HasName("PK__statusPr__5824D46AFF141759");

                entity.ToTable("statusPrijave");

                entity.Property(e => e.SifStatusa).HasColumnName("sifStatusa");

                entity.Property(e => e.NazivStatusa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivStatusa");
            });

            modelBuilder.Entity<StatusStavke>(entity =>
            {
                entity.HasKey(e => e.SifStatusa)
                    .HasName("PK__StatusSt__5824D46ACF92EECB");

                entity.ToTable("StatusStavke");

                entity.Property(e => e.SifStatusa).HasColumnName("sifStatusa");

                entity.Property(e => e.NazivStatusa)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivStatusa");

                entity.Property(e => e.OpisStatusa)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("opisStatusa");
            });

            modelBuilder.Entity<Stavka>(entity =>
            {
                entity.HasKey(e => e.SifStavke)
                    .HasName("PK__Stavka__497E4DA9019C274C");

                entity.ToTable("Stavka");

                entity.Property(e => e.SifStavke).HasColumnName("sifStavke");

                entity.Property(e => e.IznosPdv).HasColumnName("iznosPDV");

                entity.Property(e => e.JedCijena).HasColumnName("jedCijena");

                entity.Property(e => e.JedCijenaSaPdv).HasColumnName("jedCijenaSaPDV");

                entity.Property(e => e.Kolicina).HasColumnName("kolicina");

                entity.Property(e => e.NapomenaNarucitelja)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("napomenaNarucitelja");

                entity.Property(e => e.NapomenaPonuditelja)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("napomenaPonuditelja");

                entity.Property(e => e.NazivStavke)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivStavke");

                entity.Property(e => e.SifTroskovnik).HasColumnName("sifTroskovnik");

                entity.Property(e => e.SifVrstaStavke).HasColumnName("sifVrstaStavke");

                entity.Property(e => e.StopaPdv).HasColumnName("stopaPDV");

                entity.Property(e => e.UkCijena).HasColumnName("ukCijena");

                entity.Property(e => e.UkCijenaSaPdv).HasColumnName("ukCijenaSaPDV");

                entity.HasOne(d => d.SifTroskovnikNavigation)
                    .WithMany(p => p.Stavkas)
                    .HasForeignKey(d => d.SifTroskovnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stavka__sifTrosk__2C3393D0");

                entity.HasOne(d => d.SifVrstaStavkeNavigation)
                    .WithMany(p => p.Stavkas)
                    .HasForeignKey(d => d.SifVrstaStavke)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stavka__sifVrsta__2D27B809");
            });

            modelBuilder.Entity<StavkaPlanaNabave>(entity =>
            {
                entity.HasKey(e => e.SifStavke)
                    .HasName("PK__StavkaPl__497E4DA9765CA98D");

                entity.ToTable("StavkaPlanaNabave");

                entity.Property(e => e.SifStavke).HasColumnName("sifStavke");

                entity.Property(e => e.EvidencijskiBroj)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("evidencijskiBroj");

                entity.Property(e => e.Napomena)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("napomena");

                entity.Property(e => e.PredmetNabave)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("predmetNabave");

                entity.Property(e => e.SifPlanaNabave).HasColumnName("sifPlanaNabave");

                entity.Property(e => e.SifStatusa).HasColumnName("sifStatusa");

                entity.Property(e => e.VrijediDo)
                    .HasColumnType("date")
                    .HasColumnName("vrijediDo");

                entity.Property(e => e.VrijediOd)
                    .HasColumnType("date")
                    .HasColumnName("vrijediOd");

                entity.Property(e => e.VrijednostNabave).HasColumnName("vrijednostNabave");

                entity.Property(e => e.VrstaPostupka)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vrstaPostupka");

                entity.HasOne(d => d.SifPlanaNabaveNavigation)
                    .WithMany(p => p.StavkaPlanaNabaves)
                    .HasForeignKey(d => d.SifPlanaNabave)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StavkaPla__sifPl__534D60F1");

                entity.HasOne(d => d.SifStatusaNavigation)
                    .WithMany(p => p.StavkaPlanaNabaves)
                    .HasForeignKey(d => d.SifStatusa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StavkaPla__sifSt__5441852A");
            });

            modelBuilder.Entity<StrucnaSprema>(entity =>
            {
                entity.HasKey(e => e.SifStrucneSpreme)
                    .HasName("PK__StrucnaS__478DA6E059A7640E");

                entity.ToTable("StrucnaSprema");

                entity.Property(e => e.SifStrucneSpreme).HasColumnName("sifStrucneSpreme");

                entity.Property(e => e.NazivStrucneSpreme)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivStrucneSpreme");
            });

            modelBuilder.Entity<Strucnjak>(entity =>
            {
                entity.HasKey(e => e.Oibstrucnjaka)
                    .HasName("PK__Strucnja__716067F016C93B48");

                entity.ToTable("Strucnjak");

                entity.Property(e => e.Oibstrucnjaka)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBstrucnjaka");

                entity.Property(e => e.BrojMobitelaStrucnjaka)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("brojMobitelaStrucnjaka");

                entity.Property(e => e.EmailStucnjaka)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("emailStucnjaka");

                entity.Property(e => e.ImeStrucnjaka)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("imeStrucnjaka");

                entity.Property(e => e.Oibponuditelja)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("OIBponuditelja");

                entity.Property(e => e.PrezimeStrucnjaka)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("prezimeStrucnjaka");

                entity.Property(e => e.SifGrada).HasColumnName("sifGrada");

                entity.Property(e => e.SifStrucneSpreme).HasColumnName("sifStrucneSpreme");

                entity.HasOne(d => d.OibponuditeljaNavigation)
                    .WithMany(p => p.Strucnjaks)
                    .HasForeignKey(d => d.Oibponuditelja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Strucnjak__OIBpo__46E78A0C");

                entity.HasOne(d => d.SifGradaNavigation)
                    .WithMany(p => p.Strucnjaks)
                    .HasForeignKey(d => d.SifGrada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Strucnjak__sifGr__48CFD27E");

                entity.HasOne(d => d.SifStrucneSpremeNavigation)
                    .WithMany(p => p.Strucnjaks)
                    .HasForeignKey(d => d.SifStrucneSpreme)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Strucnjak__sifSt__47DBAE45");
            });

            modelBuilder.Entity<Troskovnik>(entity =>
            {
                entity.HasKey(e => e.SifTroskovnik)
                    .HasName("PK__Troskovn__9D6D8BF0945BA5E9");

                entity.ToTable("Troskovnik");

                entity.Property(e => e.SifTroskovnik).HasColumnName("sifTroskovnik");

                entity.Property(e => e.IspravnoPopunjen).HasColumnName("ispravnoPopunjen");

                entity.Property(e => e.SifValuta).HasColumnName("sifValuta");

                entity.HasOne(d => d.SifValutaNavigation)
                    .WithMany(p => p.Troskovniks)
                    .HasForeignKey(d => d.SifValuta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Troskovni__sifVa__29572725");
            });

            modelBuilder.Entity<Valutum>(entity =>
            {
                entity.HasKey(e => e.SifValuta)
                    .HasName("PK__Valuta__7AD2A96A72FB78E1");

                entity.Property(e => e.SifValuta).HasColumnName("sifValuta");

                entity.Property(e => e.ImeValute)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("imeValute");

                entity.Property(e => e.OznValute)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("oznValute");
            });

            modelBuilder.Entity<VrijednostiKriterija>(entity =>
            {
                entity.HasKey(e => new { e.SifNatjecaja, e.SifKriterija })
                    .HasName("PK__Vrijedno__22D5CB0C4CD7C568");

                entity.ToTable("VrijednostiKriterija");

                entity.Property(e => e.SifNatjecaja).HasColumnName("sifNatjecaja");

                entity.Property(e => e.SifKriterija).HasColumnName("sifKriterija");

                entity.Property(e => e.MaksBrojBodova).HasColumnName("maksBrojBodova");

                entity.Property(e => e.TezinskaOcjena).HasColumnName("tezinskaOcjena");

                entity.HasOne(d => d.SifKriterijaNavigation)
                    .WithMany(p => p.VrijednostiKriterijas)
                    .HasForeignKey(d => d.SifKriterija)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vrijednos__sifKr__6477ECF3");

                entity.HasOne(d => d.SifNatjecajaNavigation)
                    .WithMany(p => p.VrijednostiKriterijas)
                    .HasForeignKey(d => d.SifNatjecaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vrijednos__sifNa__6383C8BA");
            });

            modelBuilder.Entity<VrstaDokumentum>(entity =>
            {
                entity.HasKey(e => e.SifVrsteDokumenta)
                    .HasName("PK__VrstaDok__8DD6EE697AD3920B");

                entity.Property(e => e.SifVrsteDokumenta).HasColumnName("sifVrsteDokumenta");

                entity.Property(e => e.NazivVrsteDokumenta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivVrsteDokumenta");
            });

            modelBuilder.Entity<VrstaKompetencije>(entity =>
            {
                entity.HasKey(e => e.SifVrsteKompetencije)
                    .HasName("PK__VrstaKom__C316CEA20B16E792");

                entity.ToTable("VrstaKompetencije");

                entity.Property(e => e.SifVrsteKompetencije).HasColumnName("sifVrsteKompetencije");

                entity.Property(e => e.NazivVrsteKompetencije)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivVrsteKompetencije");
            });

            modelBuilder.Entity<VrstaKontaka>(entity =>
            {
                entity.HasKey(e => e.SifVrsteKontakta)
                    .HasName("PK__VrstaKon__924C50272E3E06DF");

                entity.ToTable("VrstaKontaka");

                entity.Property(e => e.SifVrsteKontakta).HasColumnName("sifVrsteKontakta");

                entity.Property(e => e.NazivVrsteKontakta)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nazivVrsteKontakta");
            });

            modelBuilder.Entity<VrstaStavke>(entity =>
            {
                entity.HasKey(e => e.SifVrsteStavke)
                    .HasName("PK__VrstaSta__5E4EC5914F760940");

                entity.ToTable("VrstaStavke");

                entity.Property(e => e.SifVrsteStavke).HasColumnName("sifVrsteStavke");

                entity.Property(e => e.NazivVrsteStavke)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivVrsteStavke");
            });

            modelBuilder.Entity<Zakon>(entity =>
            {
                entity.HasKey(e => e.SifZakon)
                    .HasName("PK__Zakon__288A37229A7E01E7");

                entity.ToTable("Zakon");

                entity.Property(e => e.SifZakon).HasColumnName("sifZakon");

                entity.Property(e => e.NazivZakona)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nazivZakona");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
