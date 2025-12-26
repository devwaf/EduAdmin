using System.ComponentModel.DataAnnotations;

namespace EduAdmin.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}