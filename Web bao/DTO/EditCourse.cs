using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Web_bao.Data;

namespace Web_bao.DTO
{
    public class EditCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Url {  get; set; }
        public int Category_Id { get; set; }
        public int User_id { get; set; }
    }
}
