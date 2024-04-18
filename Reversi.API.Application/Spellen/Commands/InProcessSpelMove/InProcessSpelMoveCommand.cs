using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;

namespace Reversi.API.Application.Spellen.Commands.InProcessSpelMove
{
    public class InProcessSpelMoveCommand : IRequest<BaseMoveModel>
    {
        public bool HasPassed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid? Token { get; set; }
        public Guid? SpelerToken { get; set; }
    }

    public class InProcessSpelMoveCommandHandle : IRequestHandler<InProcessSpelMoveCommand, BaseMoveModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelByIdQueryHandle> _logger;

        private readonly ISpelMovement _spelMovement;

        public InProcessSpelMoveCommandHandle(
            IRequestContext requestContext,
            IRepositoryWrapper repositoryWrapper, 
            ILogger<GetSpelByIdQueryHandle> logger,
            ISpelMovement spelMovement)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
            _spelMovement = spelMovement;
        }

        public async Task<BaseMoveModel> Handle(InProcessSpelMoveCommand request, CancellationToken cancellationToken)
        {
            if (request.SpelerToken == null && request.Token == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, no spelertoken and token found in request object.");
                return new MoveExecutedModel
                {
                    IsMovementExecuted = false
                };
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, will load in process spel entity with token: {request.Token} or with spelerToken: {request.SpelerToken}");
            var spel = await _repository.Spel.GetSpelInProcessFromSpelerOrSpelTokenAsync((Guid)request.SpelerToken, (Guid)request.Token);

            if (spel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, Could not find an in process spel entity conform the given parameters, toke: {request.Token}, spelerToken: {request.SpelerToken}");

                return new MoveExecutedModel
                {
                    IsMovementExecuted = false,
                    NotExecutedMessage = "Could not find a spel with the given parameters to perform an aciton."
                };
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 in process spel entity from the database with token: {spel.Token}.");


            bool notYourTurn = (spel.AandeBeurt == (int)Kleur.Zwart && spel.Speler2Token == request.SpelerToken) ||
                               (spel.AandeBeurt == (int)Kleur.Wit && spel.Speler1Token == request.SpelerToken);
           /* bool notYourTurn = (spel.AandeBeurt == (int)Kleur.Wit && spel.Speler1Token != request.SpelerToken) ||
                               (spel.AandeBeurt == (int)Kleur.Zwart && spel.Speler2Token != request.SpelerToken);
*/
            if (notYourTurn)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, player with token: {request.SpelerToken} from game: {spel.Token} tried to execute a move while his opponent is still trying to figure out which move to pull.");

                return new MoveExecutedModel
                {
                    IsMovementExecuted = false,
                    NotExecutedMessage = "It's not your turn yet.",
                    PlayerTurn = spel.AandeBeurt,
                };
            }

            var finishedSpelResult = await checkIfSpelIsFinished(spel, request);
            if (finishedSpelResult != null)
            {
                return finishedSpelResult;
            }
            /*bool isSpelFinished = _spelMovement.Afgelopen(spel);

            if (isSpelFinished)
            {
                _logger.LogInformation($"Request with id: {_requestContext.RequestId}, spel with token: {spel.Token} has finished.");

                spel.FinishedAt = spel.UpdatedAt = DateTime.Now;
                spel.AmountOfUpdates++;
                spel.UpdatedBy = request.SpelerToken ?? Guid.Empty;
                spel.WonBy = spel.AandeBeurt == 1 ? spel.Speler1Token : spel.Speler2Token;
                spel.LostBy = spel.AandeBeurt == 1 ? spel.Speler2Token : spel.Speler1Token;
                _repository.Spel.Update(spel);
                await _repository.SaveAsync();
                return new FinishedModel
                {
                    IsSpelFinished = true,
                    IsSpelDraw = false,

                    // TODO: Fix this because speler1 hopefully doesn't always win.
                    WinnerToken = spel.WonBy ?? throw new Exception("WinnerToken not found."),
                    LoserToken = spel.LostBy ?? throw new Exception("LoserToken not found.")
                };

                
            }*/

            if (request.HasPassed)
            {
                bool pasResult = _spelMovement.Pas(spel);

                if (pasResult)
                    _logger.LogInformation(
                        $"Request with id: {_requestContext.RequestId}, user with token: {request.SpelerToken} has passed his turn.");
                else    
                    _logger.LogInformation(
                        $"Request with id: {_requestContext.RequestId}, user with token: {request.SpelerToken} tried to pass but couldn't.");

                _repository.Spel.Update(spel);
                await _repository.SaveAsync();

                return new MoveExecutedModel
                {
                    IsMovementExecuted = pasResult,
                    PlayerTurn = spel.AandeBeurt
                };
            }
            else
            {
                List<CoordsModel> flippedResult;
                var currentPlayer = spel.AandeBeurt;

                bool moveResult = _spelMovement.DoeZet(ref spel, request.Y, request.X, out flippedResult);

                if (moveResult)
                {
                    _logger.LogInformation(
                        $"Request with id: {_requestContext.RequestId}, user with token: {request.SpelerToken} has executed his turn.");
                    if (flippedResult != null)
                    {
                        var intArrBord = spel.Bord.MapStringBordTo2DIntArr();
                        foreach (var chr in flippedResult)
                        {
                            intArrBord[chr.Y, chr.X] = currentPlayer;
                        }
                        spel.Bord = intArrBord.MapIntArrToBase64String();

                        if (spel.AandeBeurt == 1)
                        {
                            spel.AOFFBySpeler1 += (flippedResult.Count - 1);
                            //spel.AOFFBySpeler2 -= (flippedResult.Count - 1);
                        }
                        else
                        {
                            spel.AOFFBySpeler2 += (flippedResult.Count - 1);
                            //spel.AOFFBySpeler1 -= (flippedResult.Count - 1);
                        }

                    }
                }
                else
                    _logger.LogInformation(
                        $"Request with id: {_requestContext.RequestId}, user with token: {request.SpelerToken} tried to execute but couldn't.");

                // Check the flipped fiches and save the spel bord accordingly.
                

                
                _repository.Spel.Update(spel);
                await _repository.SaveAsync();

                var finishedSpelResultAfterMove = await checkIfSpelIsFinished(spel, request);
                if (finishedSpelResultAfterMove != null)
                {
                    return finishedSpelResultAfterMove;
                }

                var executeResult = new MoveExecutedModel
                {
                    NotExecutedMessage = (moveResult) ? null : "Wrong action, please try again.",
                    IsMovementExecuted = moveResult,
                    CoordsToTurnAround = moveResult ? flippedResult.ToList() : null,
                    PlayerTurn = spel.AandeBeurt
                };
                // Return Move executedmodel of current move
                return executeResult;
            }


        }

        private async Task<FinishedModel?> checkIfSpelIsFinished(Spel spel, InProcessSpelMoveCommand request)
        {
            bool isSpelFinished = _spelMovement.Afgelopen(spel);

            if (isSpelFinished)
            {
                _logger.LogInformation($"Request with id: {_requestContext.RequestId}, spel with token: {spel.Token} has finished.");

                int player1score = 0;
                int player2score = 0;

                var bord = spel.Bord.MapStringBordTo2DIntArr();
                for (int i = 0; i < bord.GetLength(0); i++)
                {
                    for (int j = 0; j < bord.GetLength(1); j++)
                    {
                        if (bord[i, j] == 2)
                            player1score++;

                        if (bord[i, j] == 1)
                            player2score++;
                    }
                }

                spel.FinishedAt = spel.UpdatedAt = DateTime.Now;
                spel.AmountOfUpdates++;
                spel.UpdatedBy = request.SpelerToken ?? Guid.Empty;
                spel.WonBy = player2score > player1score ? spel.Speler2Token : spel.Speler1Token;
                spel.LostBy = player1score > player2score ? spel.Speler2Token : spel.Speler1Token;
                spel.AOFFBySpeler1 = player1score;
                spel.AOFFBySpeler2 = player2score;
                _repository.Spel.Update(spel);
                await _repository.SaveAsync();
                return new FinishedModel
                {
                    IsSpelFinished = true,
                    IsSpelDraw = false,

                    // TODO: Fix this because speler1 hopefully doesn't always win.
                    WinnerToken = spel.WonBy ?? throw new Exception("WinnerToken not found."),
                    LoserToken = spel.LostBy ?? throw new Exception("LoserToken not found.")
                };
            }

            return null;
        }
    }
}
