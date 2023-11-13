using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BackTrackBackEnd.SQLTables;
using Microsoft.EntityFrameworkCore;

namespace BackTrackBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutineController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoutineController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("AccountRoutines/{accountId}")]
    public ActionResult<IEnumerable<RoutineWithSteps>> GetAccountRoutines(Guid accountId)
    {
        // Get the list of routines for the account with the steps included, ordered by the step order
        var routines = _context.Routines
            .Where(r => r.AccountId == accountId)
            .Include(r => r.Steps) // Make sure to include the steps
            .ToList() // Materialize the query here if lazy loading is not enabled
            .Select(r => new RoutineWithSteps // Convert the domain model to the DTO
            {
                Name = r.Name,
                Steps = r.Steps.OrderBy(s => s.Order)
                    .Select(s => new StepInfo // Convert the Step domain model to the StepInfo DTO
                    {
                        Duration = s.Duration,
                        Name = s.Name
                    }).ToList()
            }).ToList();

        if (!routines.Any())
        {
            return NotFound();
        }

        return routines;
    }

    // POST: api/Routine
    [HttpPost]
    public ActionResult<Routine> AddRoutine([FromBody] RoutineDto routineDto)
    {
        if (routineDto == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_context.Accounts.Any(a => a.Id == routineDto.AccountId))
        {
            return NotFound($"Account with ID {routineDto.AccountId} not found.");
        }

        // Check if a Routine with the same name for the AccountId already exists
        if (_context.Routines.Any(r => r.AccountId == routineDto.AccountId && r.Name == routineDto.Name))
        {
            // Return conflict status with a message indicating the routine already exists
            return Conflict($"A routine with the name \"{routineDto.Name}\" already exists for the account.");
        }

        var routine = new Routine
        {
            Id = Guid.NewGuid(),
            Name = routineDto.Name,
            AccountId = routineDto.AccountId
        };

        List<Step> steps = routineDto.Steps.Select((stepDto, index) => new Step
        {
            Id = Guid.NewGuid(),
            Name = stepDto.Name,
            Duration = stepDto.Duration,
            Order = index, // Zero-based index if you want the steps to start at order 0
            RoutineId = routine.Id // Now we can safely use routine.Id
        }).ToList();

        // Now that Steps are defined with RoutineId, we can set them to Routine
        routine.Steps = steps;

        try
        {
            _context.Routines.Add(routine);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception (adjust based on your logging framework)
            Console.WriteLine(ex.ToString());

            // Return a more informative error message
            return StatusCode(500, "An error occurred while saving the routine.");
        }

        return CreatedAtAction("GetAccountRoutines", new { accountId = routine.AccountId }, routine);
    }
}

// DTO classes for data transfer

// Named type that matches the JSON structure
public class RoutineWithSteps
{
    public string Name { get; set; }
    public List<StepInfo> Steps { get; set; }
}

// Named type for steps that only includes the required fields
public class StepInfo
{
    public int Duration { get; set; }
    public string Name { get; set; }
}

public class RoutineDto
{
    public Guid AccountId { get; set; }
    public string Name { get; set; }
    public List<StepDto> Steps { get; set; }
}

public class StepDto
{
    public int Duration { get; set; }
    public string Name { get; set; }
}
