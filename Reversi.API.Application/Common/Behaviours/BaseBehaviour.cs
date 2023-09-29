using System;
using System.Collections.Generic;
using System.Text;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application.Common.Behaviours
{
    public abstract class BaseBehaviour
    {
        protected IRequestContext BehaviourContext { get; }

        protected BaseBehaviour(IRequestContext behaviourContext)
        {
            BehaviourContext = behaviourContext;
        }
    }
}
