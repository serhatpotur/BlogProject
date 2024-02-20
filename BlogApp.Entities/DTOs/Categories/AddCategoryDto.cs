namespace BlogApp.Entities.DTOs.Categories
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public  DateTime CreatedDate { get; set; }
        public  bool isDeleted { get; set; } 

    }
}
