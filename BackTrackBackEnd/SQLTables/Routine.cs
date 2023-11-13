using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackTrackBackEnd.SQLTables;

/// <summary>
/// The Routine related information tied to an Account
/// </summary>
public class Routine
{
    /// <summary>
    /// The unique identifier for each routine
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the Routine
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// The Account identifier that the routine is tied to
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Navigation property for the linked Account
    /// </summary>
    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    /// <summary>
    /// The ordered list of steps in the routine
    /// </summary>
    public List<Step> Steps { get; set; }
}