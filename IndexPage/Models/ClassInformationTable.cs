namespace IndexPage.Models
{
    public class ClassInformationTable
    {
        public int Id { get; set; }

    
        public string? ClassName { get; set; } // nullable halde,değer verilmemiş olabilir
        public int StudentCount { get; set; }

        public string? Description { get; set; } // nullable,açıklama olmaydabilir
    }
}
