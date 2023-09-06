namespace Bouquet.Database.Interfaces
{
    /// <summary>
    /// Auditability interface
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime? CreatedOn { get; set; }
        DateTime? LastModifiedOn { get; set; }
        string? CreatedBy { get; set; }
        string? LastModifiedBy { get; set; }
    }
}
