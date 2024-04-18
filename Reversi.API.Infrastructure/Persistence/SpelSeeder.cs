using System;
using System.Collections.Generic;
using System.Text;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Infrastructure.Persistence
{
    public class SpelSeeder
    {
        private readonly RepositoryContext _context;
        private static readonly int[,] _bord = new int[8, 8]
        {
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 1, 2, 0, 0, 0 },
            {0, 0, 0, 2, 1, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public SpelSeeder(RepositoryContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            /*foreach (var item in Spellen)
            {
                _context.Spellen.Add(item);
            }

            _context.SaveChanges();*/
        }

        private static List<Spel> Spellen = new List<Spel>
        {
            // --- Five 'Open/InQueue' Spellen ---
            new Spel
            {
                Omschrijving = "On the Same Page",
                Speler1Token = Guid.Parse("dcda7fae-f3f2-41cd-bbb1-65b36476b1af"),
                Bord = _bord.MapIntArrToBase64String(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Quick and Dirty",
                Speler1Token = Guid.Parse("d21be77f-0981-4304-ba03-90b023783935"),
                Bord = _bord.MapIntArrToBase64String(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "There's No I in Team",
                Speler1Token = Guid.Parse("3096394d-bf57-4639-8353-1bcf7751630e"),
                Bord = _bord.MapIntArrToBase64String(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Elephant in the Room",
                Speler1Token = Guid.Parse("0997c272-1b50-4174-9c3b-699fc1561478"),
                Bord = _bord.MapIntArrToBase64String(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Hear, Hear",
                Speler1Token = Guid.Parse("e4f14e33-be07-489a-bb5b-80474b847a57"),
                Bord = _bord.MapIntArrToBase64String(),
                AandeBeurt = 2
            },

            // --- Three 'InProcess' Spellen ---
            new Spel
            {
                Omschrijving = "Rain on Your Parade",
                Speler1Token = Guid.Parse("88718317-21aa-472e-b4bb-2950975ad648"),
                Speler2Token = Guid.Parse("9cc4573a-b79b-4130-b4f0-b0746bc8ea21"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Not the Sharpest Tool in the Shed",
                Speler1Token = Guid.Parse("2c3d3725-6d3a-466d-af60-76d54a16c65d"),
                Speler2Token = Guid.Parse("57a3fced-0c05-4b45-856c-55304d89f584"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Easy As Pie",
                Speler1Token = Guid.Parse("d513e2c8-c90a-4b2d-b04a-ea230a59e5ad"),
                Speler2Token = Guid.Parse("6def24b6-b36c-4c78-a370-54d11d96c8e9"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                AandeBeurt = 2
            },

            // --- Nine 'Finished' Spellen ---
            new Spel
            {
                Omschrijving = "Keep Your Eyes Peeled",
                Speler1Token = Guid.Parse("2c3d3725-6d3a-466d-af60-76d54a16c65d"),
                Speler2Token = Guid.Parse("849237c3-92a4-4d4c-a2f4-71f57d69b654"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "In the Red",
                Speler1Token = Guid.Parse("3bf95975-c45d-4d8d-b07b-6807eb297a4b"),
                Speler2Token = Guid.Parse("c9051580-4dd0-45f5-a506-b763420996d5"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "A Fool and His Money are Soon Parted",
                Speler1Token = Guid.Parse("20266d6a-e0bd-4716-b48f-cb62ea1af539"),
                Speler2Token = Guid.Parse("dbd63f1e-d960-4c32-82f9-0525f081e7cd"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Hit Below The Belt",
                Speler1Token = Guid.Parse("840397d7-a6d8-4d28-b0db-0f199b7b28fb"),
                Speler2Token = Guid.Parse("a87394ee-eb15-49c5-ad4c-69a53f0fabaf"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "My Cup of Tea",
                Speler1Token = Guid.Parse("840397d7-a6d8-4d28-b0db-0f199b7b28fb"),
                Speler2Token = Guid.Parse("c5d9a06b-de89-4f75-b2c5-bef1ec2ade30"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(5),
                AandeBeurt = 2
            },

            new Spel
            {
                Omschrijving = "Up In Arms",
                Speler1Token = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                Speler2Token = Guid.Parse("dbd63f1e-d960-4c32-82f9-0525f081e7cd"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(28),
                AandeBeurt = 2,
                WonBy = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                LostBy = Guid.Parse("dbd63f1e-d960-4c32-82f9-0525f081e7cd"),
            },

            new Spel
            {
                Omschrijving = "Swinging For the Fences",
                Speler1Token = Guid.Parse("840397d7-a6d8-4d28-b0db-0f199b7b28fb"),
                Speler2Token = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(7),
                AandeBeurt = 2,
                WonBy = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                LostBy = Guid.Parse("840397d7-a6d8-4d28-b0db-0f199b7b28fb"),
            },

            new Spel
            {
                Omschrijving = "Long In The Tooth",
                Speler1Token = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                Speler2Token = Guid.Parse("c5d9a06b-de89-4f75-b2c5-bef1ec2ade30"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(2),
                AandeBeurt = 2,
                WonBy = Guid.Parse("c5d9a06b-de89-4f75-b2c5-bef1ec2ade30"),
                LostBy = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
            },

            // -- One Draw Spel -- 
            new Spel
            {
                Omschrijving = "Long In The Tooth",
                Speler1Token = Guid.Parse("b3fcebbd-0cd4-480e-b751-6e4be18e4cad"),
                Speler2Token = Guid.Parse("c5d9a06b-de89-4f75-b2c5-bef1ec2ade30"),
                Bord = _bord.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.ToUniversalTime(),
                FinishedAt = DateTime.Now.ToUniversalTime() + TimeSpan.FromMinutes(14),
                AandeBeurt = 2,
                WonBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                LostBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            },
        };
    }
}
