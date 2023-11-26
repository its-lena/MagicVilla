using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO{Id=1, Name="Pool View", Occupancy=3, Sqft=100},
            new VillaDTO{Id=2, Name="Beach View", Sqft= 200, Occupancy=2}
        };
    }
}
