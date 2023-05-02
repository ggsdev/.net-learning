using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Minimo de 3, máximo de 40")]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
    }

}