using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Domain.Common.Exceptions
{
    public class SelfParticipationException : Exception
    {
        public SelfParticipationException(Guid token, Guid speler1Token, Guid speler2Token)
            : base(
                $"Speler2: {speler2Token} tried to participate in a game created by himself. token: {token}, createdby: {speler1Token}")

        {

        }
    }
}
