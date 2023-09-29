using System;
using System.ComponentModel.DataAnnotations;

namespace Reversi.API.DataTransferObjects
{
    public class SpelCreationDto
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Description must be a minimum of 3 characters long and has as maximum size of 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "First player token is required")]
        public Guid Speler1Token { get; set; }
    }
}
