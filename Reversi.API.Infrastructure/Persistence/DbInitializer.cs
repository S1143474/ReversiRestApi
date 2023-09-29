using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Reversi.API.Infrastructure.Persistence;

namespace Reversi.API.Infrastructure.Persistence
{
    public class DbInitializer
    {
        private readonly RepositoryContext _context;

        public DbInitializer(RepositoryContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Spellen.Any())
            {
                var seeder = new SpelSeeder(_context);
                seeder.Seed();
            }
        }

    }
}
