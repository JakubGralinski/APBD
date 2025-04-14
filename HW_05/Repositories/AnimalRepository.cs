using APBD_05.Models;

namespace APBD_05.Repositories;

using System.Collections.Generic;
using System.Linq;

public class AnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals;

    public AnimalRepository()
    {
        // dummy data init
        _animals = new List<Animal>
        {
            new Animal { Id = 1, Name = "Buddy", Category = "Dog", Weight = 20.5, FurColor = "Brown" },
            new Animal { Id = 2, Name = "Mittens", Category = "Cat", Weight = 8.2, FurColor = "Black" }
        };
    }

    public IEnumerable<Animal> GetAll() => _animals;

    public Animal GetById(int id) => _animals.FirstOrDefault(a => a.Id == id);

    public Animal Add(Animal animal)
    {
        animal.Id = _animals.Any() ? _animals.Max(a => a.Id) + 1 : 1;
        _animals.Add(animal);
        return animal;
    }

    public Animal Update(int id, Animal updatedAnimal)
    {
        var animal = GetById(id);
        if (animal == null)
            return null;

        animal.Name = updatedAnimal.Name;
        animal.Category = updatedAnimal.Category;
        animal.Weight = updatedAnimal.Weight;
        animal.FurColor = updatedAnimal.FurColor;
        return animal;
    }

    public void Delete(int id)
    {
        var animal = GetById(id);
        if (animal != null)
            _animals.Remove(animal);
    }

    public bool AnimalExists(int id) => _animals.Any(a => a.Id == id);
}