namespace MainAPI.Models.DTOs
{
    public class MatriculaMultipleDto
    {
        public int IdEstudiante { get; set; }
        public List<int> IdsCursosHabilitados { get; set; } = new List<int>();
    }

}
