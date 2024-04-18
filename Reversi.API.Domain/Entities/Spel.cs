using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Reversi.API.Domain.Common;
using Reversi.API.Domain.Enums;

namespace Reversi.API.Domain.Entities
{
    [Table("spel")]
    public class Spel : BaseAuditableEntity
    {

        [Key]
        [Required(ErrorMessage = "Speltoken is required")]
        public Guid Token { get; set; }


        [Column("Description")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "De spel omschrijving kan maximaal 100 en moet minimaal 3 tekens bevatten.")]
        public string Omschrijving { get; set; }


        [Required(ErrorMessage = "Speler1token is required")]
        public Guid Speler1Token { get; set; }

        public Guid? Speler2Token { get; set; }

        [Required(ErrorMessage = "Bord is required")]
        public string Bord { get; set; }

        [Column("Turn")]
        public int AandeBeurt { get; set; }
        public DateTime? StartedAt { get; set; }

        [Column("EndedAt")]
        public DateTime? FinishedAt { get; set; }

        public bool AllowQuickStart { get; set; }
        public Guid? WonBy { get; set; }
        public Guid? LostBy { get; set; }
        public int AOFFBySpeler1 { get; set; }
        public int AOFFBySpeler2 { get; set; }
    }
}
