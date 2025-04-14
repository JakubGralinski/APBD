using APBD_05.Models;

namespace APBD_05.Repositories;

using System.Collections.Generic;

public interface IVisitRepository
{
    IEnumerable<Visit> GetVisitsByAnimalId(int animalId);
    Visit AddVisit(Visit visit);
    void RemoveVisitsByAnimalId(int animalId);
}