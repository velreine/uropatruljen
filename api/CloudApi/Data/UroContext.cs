using Microsoft.EntityFrameworkCore;

using CommonData.Model.Entity;
#pragma warning disable CS1591

namespace CloudApi.Data;

public class UroContext : DbContext
{
    public UroContext (DbContextOptions<UroContext> options) : base(options) { }

    public DbSet<Component> Components => Set<Component>();
    public DbSet<Pin> Pins => Set<Pin>();
    public DbSet<HardwareLayout> HardwareLayouts => Set<HardwareLayout>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Home> Homes => Set<Home>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<ComponentState> ComponentStates => Set<ComponentState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Since we don't want to expose each individual derived type of ComponentState (by making a DbSet of them)
        // , we must tell EF they exist.
        modelBuilder.Entity<RgbComponentState>();
        
        // Let EF do its job.
        base.OnModelCreating(modelBuilder);

    }
}