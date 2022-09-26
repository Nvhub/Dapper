using System.ComponentModel.DataAnnotations;

namespace DapperProject.Core.Entities
{
    public class UserAuth
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
