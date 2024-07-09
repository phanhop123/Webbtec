namespace Web_bao.DTO
{
    public class Courses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile Image { get; set; }
        public int User_id { get; set; }
        public int Category_Id { get; set; }

    }
}
