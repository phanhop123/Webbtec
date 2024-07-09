using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Web_bao.Data
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [ForeignKey(nameof(Category))]
        [DisplayName("Category")]
        public int Category_Id { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(User))]
        [DisplayName("User")]
        public int User_id { get; set; }
        public virtual User User { get; set; }
    }
}
