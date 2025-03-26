namespace IndexPage.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; } //get okunabilir,set de yazılabilir için

        public string? ClassName { get; set; } //ClassName de hata veriyordu,gpt ye sordum null yapmayı önerdi 
        public int StudentCount { get; set; }

        public string? Description { get; set; }//ClassName ile aynı sebepten null değerinde

        
    }
}
