using System.Text.Json.Serialization;

namespace IndexPage.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [JsonIgnore] 
        public bool IsSelected { get; set; } 

        public string ClassName { get; set; } = string.Empty;

        public int StudentCount { get; set; } 

        public string Description { get; set; } = string.Empty; 
    }
}