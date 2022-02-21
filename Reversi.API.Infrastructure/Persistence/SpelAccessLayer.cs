using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;
using Reversi.API.Infrastructure.Persistence.SQLCommands;

namespace Reversi.API.Infrastructure.Persistence
{
    public class SpelAccessLayer : ISpelRepository
    {
        public async Task<SpelList> LoadSpellenAsync(CancellationToken token)
        {
            var result = await new SQLSelectCommand()
                .Select()
                .From("Spel")
                .Build()
                .Execute();

            /*SpelList list = new SpelList();
            foreach (var item in result)
                list.Spellen.Add(item);*/
            return null;
        }
    }
}
