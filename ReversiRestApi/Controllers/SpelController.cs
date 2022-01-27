using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReversiRestApi.DataTransferModels;
using ReversiRestApi.DataTransferModels.Spel;
using ReversiRestApi.Filters;

namespace ReversiRestApi.Controllers
{
    [Route("api/")]
    [ApiKeyAuth]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository iRepository;
        private readonly ILogger<SpelController> _logger;

        public SpelController(ISpelRepository repository, ILogger<SpelController> logger)
        {
            iRepository = repository;
           _logger = logger;
        }
        
        /// <summary>
        /// Retrieves all reversi game descriptions with a speler2token of null
        /// </summary>
        /// <returns></returns>
        //[HttpGet("Spel")] // api/Spel
        // TODO Check if this is still in use...
        public async Task<ActionResult<IEnumerable<string>>> GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken token)
        {
            List<string> descriptionList = new List<string>();

            foreach (Spel spel in await iRepository.GetSpellenAsync(token))
            {
                if (spel.Speler2Token == null)
                    descriptionList.Add(spel.Omschrijving);
            }

            return descriptionList;
        }

        /// <summary>
        /// Retrieves a list of games reversi with waiting players.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Spel")]
        public async Task<ActionResult<List<SpelJsonObj>>> GetGameReversiWithWaitingPlayers(CancellationToken token)
        {
            _logger.LogInformation("Retrieving Reversi Games");

            List<SpelJsonObj> result = new List<SpelJsonObj>();

            foreach (var spel in await iRepository.GetSpellenAsync(token))
            {
                if (spel.Speler2Token == null)
                    result.Add(new SpelJsonObj(spel));
                _logger.LogInformation(spel.Omschrijving);
            }

            return result;
        }

        /// <summary>
        /// Adds a new game reversi to the repository object.
        /// </summary>
        /// <param name="placeGameJsonObj"></param>
        [HttpPost("Spel")]
        public ActionResult PlaceNewGameReversi(CancellationToken token, [FromBody] PlaceGameJsonObj placeGameJsonObj)
        {
            if (!string.IsNullOrWhiteSpace(placeGameJsonObj.Description) && !string.IsNullOrWhiteSpace(placeGameJsonObj.PlayerToken))
            {
                Spel spel = new Spel()
                {
                    Speler1Token = placeGameJsonObj.PlayerToken,
                    Omschrijving = placeGameJsonObj.Description,
                    Token = Guid.NewGuid().ToString()
                };

                iRepository.AddSpel(token, spel);
                return Ok();
            }
            else return BadRequest();
        }

        /// <summary>
        /// Makes it possible for an opponent to join the game.
        /// </summary>
        /// <param name="joinGameObj">Object with all body parameters</param>
        [HttpPut("Spel/join")]
        public async Task<ActionResult> JoinGameReversi(CancellationToken token, [FromBody] JoinGameObj joinGameObj)
        {
            if (joinGameObj != null && !string.IsNullOrWhiteSpace(joinGameObj.SpelToken) && !string.IsNullOrWhiteSpace(joinGameObj.Speler2Token))
            {
                Spel spel = await iRepository.GetSpel(token, joinGameObj.SpelToken);
                
                if (spel.Speler1Token != joinGameObj.Speler2Token)
                {
                    await iRepository.JoinSpel(token, joinGameObj);
                    return Ok();
                } else
                {
                    //_logger.LogWarning($"Player: { joinGameObj.Speler2Token } tried to join a game: {joinGameObj.SpelToken} created by the same player.");
                }
            }
            //_logger.LogWarning($"[JOIN] - Some parameters were not entered: - { joinGameObj.SpelToken} - { joinGameObj.Speler2Token }.");
            return BadRequest();
        }

        /// <summary>
        /// Retrieve Game via speltoken.
        /// </summary>
        /// <param name="id">Token</param>
        /// <returns></returns>
       // [Route("api/Spel/")]
        [HttpGet("Spel/{id}")]
        public async Task<ActionResult<SpelJsonObj>> RetrieveGameReversi(CancellationToken token, string id)
        {
            Spel result = await iRepository.GetSpel(token, id);

            return (result != null) ? new SpelJsonObj(result) : null;
        }

        /// <summary>
        /// Retrieve Game via speler1Token OR speler2Token
        /// </summary>
        /// <param name="id">SpelerToken</param>
        /// <returns></returns>
        [HttpGet("SpelSpeler/{id}")]
        public async Task<ActionResult<SpelJsonObj>> RetrieveGameReversiViaSpelerToken(CancellationToken token, string id)
        {
            Spel result = await GetSpelFromSpelerToken(token, id);

            return (result != null) ? new SpelJsonObj(result) : null;
        }


        /// <summary>
        /// Retrieve current player turn.
        /// </summary>
        /// <param name="id">SpelerToken</param>
        /// <returns>-1 when no spel is found <br></br> 0 for 'Geen' (Spel not yet started)<br></br>1 for 'Wit'<br>2 for 'Zwart'</br></returns>
        [HttpGet("Spel/Beurt/{speltoken}")]
        public async Task<ActionResult<BeurtJsonObj>> GetNextPlayerTurn(CancellationToken token, string speltoken)
        {
            Spel result = await iRepository.GetSpel(token, speltoken);

            return new BeurtJsonObj() { Beurt = (result != null) ? (int)result.AandeBeurt : -1};
        }

        /// <summary>
        /// Verifies if player can proceed with move and places the move.
        /// </summary>
        /// <param name="moveJsonObj"></param>
        /// <returns></returns>
        [HttpPut("Spel/Zet")]
        public async Task<ActionResult<BaseTransferModel>> PutDoMove(CancellationToken token, [FromBody] MoveJsonObj moveJsonObj)
        {
            if (moveJsonObj.SpelerToken == null && moveJsonObj.Token == null)
                return new PutDoMoveExecutedJsonObj() { IsPlaceExecuted = false };

            Spel spel = await GetSpelFromSpelerOrSpelToken(token, moveJsonObj.SpelerToken, moveJsonObj.Token);

            bool isFinished = spel.Afgelopen();
            if (isFinished)
                return new SpelFinishedDTO { IsSpelFinished = true, IsDraw = false, WinnerToken = spel.Speler1Token, LoserToken = spel.Speler2Token};

            //TODO: check on null spel
            if (moveJsonObj.HasPassed)
            {
                bool result = spel.Pas();
                await iRepository.UpdateSpel(token, spel);
                var a = new PutDoMoveExecutedJsonObj() { IsPlaceExecuted = result, AanDeBeurt = (int)spel.AandeBeurt };
                return a;
            }
            else
            {
                bool result = spel.DoeZet(moveJsonObj.Y, moveJsonObj.X);
                await iRepository.UpdateSpel(token, spel);
                return new PutDoMoveExecutedJsonObj() { IsPlaceExecuted =  result, FichesToTurnAround = spel.CellsToFlip, AanDeBeurt = (int)spel.AandeBeurt };
            }
        }


        /// <summary>
        /// Player Aandebeurt gives up and loses.
        /// </summary>
        /// <param name="giveUpJsonObj"></param>
        /// <returns></returns>
        [HttpPut("Spel/Opgeven")]
        public async Task<ActionResult<bool>> GiveUp(CancellationToken token, [FromBody] GiveUpJsonObj giveUpJsonObj)
        {
            if (string.IsNullOrWhiteSpace(giveUpJsonObj.SpelerToken) && string.IsNullOrWhiteSpace(giveUpJsonObj.Token))
                return false;

            // TODO: integrate give up functionality
            Spel spel = await GetSpelFromSpelerOrSpelToken(token, giveUpJsonObj.SpelerToken, giveUpJsonObj.Token);
            
            return (spel != null);
        }

        [HttpGet("Spel/SpelToken")]
        public async Task<ActionResult> GetSpelTokenFromSpelerToken(CancellationToken token, string spelerToken)
        {
            if (string.IsNullOrWhiteSpace(spelerToken))
                return NotFound();

            var spel = await iRepository.SelectSpelTokenViaSpelerToken(token, spelerToken);

            if (spel == null)
                return NotFound();

            return Ok(spel.Token);
        }

        [HttpGet("Spel/SpelFinished/{spelToken}")]
        public async Task<ActionResult> GetSpelFinishedResults(CancellationToken token, string spelToken)
        {
            if (string.IsNullOrWhiteSpace(spelToken))
                return NotFound();

            var spel = await iRepository.GetSpel(token, spelToken);

            if (spel == null)
                return NotFound();

            var winner = spel.OverwegendeKleur();

            // TODO: Make objects for these results. with possibilities for amount of fiches flipped etc.
            if (winner == ISpel.Kleur.Geen)
                return Ok("Gelijk Spel");

            if (winner == ISpel.Kleur.Wit)
                return Ok("Wit Wint");

            return Ok("Zwart wint");
        }

        /// <summary>
        /// Returns a Spel from spelerTOken or spelToken.
        /// </summary>
        /// <param name="spelerToken"></param>
        /// <param name="spelToken"></param>
        /// <returns></returns>
        private async Task<Spel> GetSpelFromSpelerOrSpelToken(CancellationToken token, string spelerToken, string spelToken)
        {
           return (!string.IsNullOrWhiteSpace(spelToken)) ? await iRepository.GetSpel(token, spelToken) : await GetSpelFromSpelerToken(token, spelerToken);
        }

        /// <summary>
        /// Retrieves a spel based on a spelerToken
        /// </summary>
        /// <param name="spelerToken"></param>
        /// <returns></returns>
        private async Task<Spel> GetSpelFromSpelerToken(CancellationToken token, string spelerToken)
        {
            var speler1 = await iRepository.GetSpelFromSpeler1(token, spelerToken);
            var speler2 = await iRepository.GetSpelFromSpeler2(token, spelerToken);

            return speler1 ?? speler2;
            //iRepository.GetSpellen()
            /*return (await iRepository.GetSpellenAsync(token)).Where(spel => (spel.Speler1Token != null && spel.Speler1Token.Equals(spelerToken)) || (spel.Speler2Token != null && spel.Speler2Token.Equals(spelerToken))).Select(spel => spel).FirstOrDefault();*/
        }
    }
}
