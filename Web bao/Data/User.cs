using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Data;

namespace Web_bao.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? LastName { get; set; }    
        public string? FistName { get; set; }    
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string? City { get; set; }
        public string? Mobile { get; set; }
        public int? Age { get; set; }
        public int Role {  get; set; }

        public string? Image { get; set; }

		[ForeignKey(nameof(Major))]
		[DisplayName("Major")]
		public int Major_id { get; set; }
		public virtual Major Major { get; set; }

	}
}
