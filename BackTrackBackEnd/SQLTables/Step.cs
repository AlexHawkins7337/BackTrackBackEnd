using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

namespace BackTrackBackEnd.SQLTables;

/// <summary>
/// The Step information tied to a Routine, with a specific order
/// </summary>
public class Step
{
    /// <summary>
    /// The unique identifier for each step
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// The duration of the step in seconds
    /// </summary>
    [Required]
    public int Duration { get; set; }

    /// <summary>
    /// The name of the Step
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// The Routine identifier that step is tied to
    /// </summary>
    [Required]
    public Guid RoutineId { get; set; }

    /// <summary>
    /// The order of the step within its routine
    /// </summary>
    [Required]
    public int Order { get; set; }

    /// <summary>
    /// Navigation property for the linked Routine
    /// </summary>
    [ForeignKey("RoutineId")]
    [JsonIgnore]
    public Routine Routine { get; set; }
}