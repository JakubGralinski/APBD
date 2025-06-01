using System.Collections.Generic;

namespace APBD_09_HW.DTOs
{
    public class PaginatedTripsResponseDto
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<TripDto> Trips { get; set; } = new();
    }
} 