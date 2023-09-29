using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.API.Application.Common.Interfaces
{
    public interface IRepositoryWrapper
    {
        ISpelRepository Spel { get; }

        void Save();
        Task SaveAsync();
    }
}
