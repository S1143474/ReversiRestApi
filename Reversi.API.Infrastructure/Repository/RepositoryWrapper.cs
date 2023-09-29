using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;
using Reversi.API.Infrastructure.Persistence;

namespace Reversi.API.Infrastructure.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private ISpelRepository _spel;

        private ISortHelper<Spel> _spelSortHelper;

        public ISpelRepository Spel
        {
            get { return _spel ??= new SpelRepository(_repositoryContext, _spelSortHelper);  }
        }
        
        public RepositoryWrapper(RepositoryContext repositoryContext,
            ISortHelper<Spel> spelSortHelper)
        {
            _repositoryContext = repositoryContext;
            _spelSortHelper = spelSortHelper;
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }

        public async Task SaveAsync()
        { 
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
