using APBD_05.Models;

namespace APBD_05.Repositories;

using System.Collections.Generic;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAll();
    Animal GetById(int id);
    Animal Add(Animal animal);
    Animal Update(int id, Animal animal);
    void Delete(int id);
    bool AnimalExists(int id);
}