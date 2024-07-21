namespace Marketplace.BaseLibrary.Entity.Base;

public abstract class BaseEntity
{
    public DateTime? Inserted { get; set; }
    
    public DateTime? Updated { get; set; }
    
    public DateTime? Created { get; set; }
}