using APBD_05.Models;
using APBD_05.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IVisitRepository _visitRepository;

    public AnimalsController(IAnimalRepository animalRepository, IVisitRepository visitRepository)
    {
        _animalRepository = animalRepository;
        _visitRepository = visitRepository;
    }

    // GET
    [HttpGet]
    public IActionResult GetAll() => Ok(_animalRepository.GetAll());
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var animal = _animalRepository.GetById(id);
        return animal == null ? NotFound("Animal not found") : Ok(animal);
    }

    // POST
    [HttpPost]
    public IActionResult Create([FromBody] Animal animal)
    {
        var createdAnimal = _animalRepository.Add(animal);
        return CreatedAtAction(nameof(GetById), new { id = createdAnimal.Id }, createdAnimal);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Animal animal)
    {
        var updatedAnimal = _animalRepository.Update(id, animal);
        return updatedAnimal == null ? NotFound("Animal not found") : Ok(updatedAnimal);
    }

    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_animalRepository.AnimalExists(id))
            return NotFound("Animal not found");

        _animalRepository.Delete(id);
        _visitRepository.RemoveVisitsByAnimalId(id);
        return NoContent();
    }

    // GET visits
    [HttpGet("{id}/visits")]
    public IActionResult GetVisitsForAnimal(int id)
    {
        if (!_animalRepository.AnimalExists(id))
            return NotFound("Animal not found");

        var visits = _visitRepository.GetVisitsByAnimalId(id);
        return Ok(visits);
    }

    // POST
    [HttpPost("{id}/visits")]
    public IActionResult AddVisit(int id, [FromBody] Visit visit)
    {
        if (!_animalRepository.AnimalExists(id))
            return NotFound("Animal not found");
        
        visit.AnimalId = id;
        var createdVisit = _visitRepository.AddVisit(visit);
        return CreatedAtAction(nameof(GetVisitsForAnimal), new { id = id }, createdVisit);
    }
}