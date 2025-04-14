using APBD_05.Models;

namespace APBD_05.Repositories;

using System.Collections.Generic;
using System.Linq;

public class VisitRepository : IVisitRepository
{
    private readonly List<Visit> _visits;

    public VisitRepository()
    {
        _visits = new List<Visit>();
    }

    public IEnumerable<Visit> GetVisitsByAnimalId(int animalId)
    {
        return _visits.Where(v => v.AnimalId == animalId);
    }

    public Visit AddVisit(Visit visit)
    {
        _visits.Add(visit);
        return visit;
    }

    public void RemoveVisitsByAnimalId(int animalId)
    {
        _visits.RemoveAll(v => v.AnimalId == animalId);
    }
}