using System.ComponentModel.DataAnnotations;

namespace BackTrackBackEnd.SQLTables;

/// <summary>
/// The Account Column information
/// </summary>
public class Account
{
    /// <summary>
    /// The unique identifier for each account
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The Username of the Account
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Username { get; set; }
    
    /// <summary>
    /// The Password of the Account
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Password { get; set; }
}