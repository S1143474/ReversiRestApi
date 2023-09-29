using System;
using System.ComponentModel.DataAnnotations;

namespace Reversi.API.DataTransferObjects
{
    public class SpelParticipateDto
    {
        [Required(ErrorMessage = "SpelToken is required.")]
        public Guid? SpelToken { get; set; }

        [Required(ErrorMessage = "Speler2Token is required")]
        public Guid? Speler2Token { get; set; }
    }
}
