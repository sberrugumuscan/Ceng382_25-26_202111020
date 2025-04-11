namespace IndexPage.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        public string ClassName { get; set; } = string.Empty; // varsayılan olarak boş bir string

        public int StudentCount { get; set; }

        public string? Description { get; set; }
    }
}
