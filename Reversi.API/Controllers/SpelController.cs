using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.DataTransferObjects;
/*using Reversi.API.Application.Spellen.Commands.CreateReversi;
using Reversi.API.Application.Spellen.Commands.DoMove;
using Reversi.API.Application.Spellen.Commands.FinishSpel;
using Reversi.API.Application.Spellen.Commands.JoinReversi;
using Reversi.API.Application.Spellen.DTO;
using Reversi.API.Application.Spellen.Queries.LoadAllReversi;
using Reversi.API.Application.Spellen.Queries.LoadReversi;
using Reversi.API.Application.Spellen.Queries.LoadReversi.Contract;
using Reversi.API.Application.Spellen.Queries.LoadReversiFinishedResults;*/
using Reversi.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Application.Spellen.Commands.CreateSPel;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove;
using Reversi.API.Application.Spellen.Commands.ParticipateSpel;
using Reversi.API.Application.Spellen.Commands.SurrenderSpel;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Application.Spellen.Queries.GetSpelById;
using Reversi.API.Application.Spellen.Queries.GetSpelBySpelerToken;
using Reversi.API.Application.Spellen.Queries.GetSpellen;
using Reversi.API.DataTransferObjects.Move;
using Reversi.API.DataTransferObjects.Requests;
using Reversi.API.Filters;
using Reversi.API.Application.Spellen.Commands.DeleteSpel;
using System.Text.Json;

namespace Reversi.API.Controllers
{
    public class SpelController : BaseController
    {
        private readonly ILogger<SpelController> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SpelController(ILogger<SpelController> logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Action GetAllSpellenAsync Retrieves all spellen Async
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>A PageList with Spel entities</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSpellenAsync([FromQuery] SpelParameters parameters)
        {
            var query = await Mediator.Send(new GetAllSpellenQuery
            {  
                Paramaters = parameters
            });

            var metadata = new
            {
                query.TotalCount,
                query.PageSize,
                query.CurrentPage,
                query.TotalPages,
                query.HasNext,
                query.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var spelResult = _mapper.Map<List<SpelDto>>(query);

            return Ok(spelResult);
        }

        /// <summary>
        /// Action that retrieves a Spel entity based on a Guid.
        /// </summary>
        /// <param name="id">Token</param>
        /// <returns>A Spel Entity</returns>
        [HttpGet("{id}", Name = "SpelById")]
        public async Task<IActionResult> GetSpelByIdAsync(Guid id)
        {
            var query = await Mediator.Send(new GetSpelByIdQuery
            {
                Id = id
            });

            var spelResult = _mapper.Map<SpelDto>(query);

            return Ok(spelResult);
        }

        /// <summary>
        /// Action for creating a new Spel entity based on a description and a speler1token (GUID)
        /// </summary>
        /// <param name="spelDto">Object for creating a new Spel entity containing a description and speler1Token</param>
        /// <returns>A newly created Spel entity</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateSpelAsync([FromBody] SpelCreationDto spelDto)
        {
            var spel = _mapper.Map<Spel>(spelDto);
            
            var command = await Mediator.Send(new CreateSpelCommand
            {
                Spel = spel
            });

            var spelResult = _mapper.Map<SpelDto>(command);

            return CreatedAtRoute("SpelById", new { id = spelResult.Token }, spelResult);
        }

        /// <summary>
        /// Action for retrieving all Spel entities which are in queue for an opponent.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>A PageList with Spel entities</returns>
        [HttpGet("queue")]
        [QueryStringConstraint("spelerToken", false)]
        [QueryStringConstraint("", true)]
        public async Task<IActionResult> GetAllSpellenInQueueAsync([FromQuery] SpelParameters parameters)
        {
            var query = await Mediator.Send(new GetAllSpellenInQueueQuery
            {
                Paramaters = parameters
            });

            var metadata = new
            {
                query.TotalCount,
                query.PageSize,
                query.CurrentPage,
                query.TotalPages,
                query.HasNext,
                query.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var spelResult = _mapper.Map<List<SpelDto>>(query);

            return Ok(spelResult);
        }

        /// <summary>
        /// Action for retrieving a specific Spel entity which is in queue for an opponent.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An in queue Spel entity.</returns>
        [HttpGet("queue/{id}")]
        public async Task<IActionResult> GetSpelInQueueByIdAsync(Guid id)
        {
            var query = await Mediator.Send(new GetSpelInQueueByIdQuery
            {
                Id = id
            });

            var spelInQueueResult = _mapper.Map<SpelDto>(query);

            return Ok(spelInQueueResult);
        }

        [HttpGet]
        [Route("queue")]
        [QueryStringConstraint("spelerToken", true)]
        [QueryStringConstraint("", false)]
        public async Task<IActionResult> GetSpelInQueueBySpelerTokenAsync([FromQuery] Guid spelerToken)
        {
            var query = await Mediator.Send(new GetSpelInQueueBySpelerTokenQuery
            {
                SpelerToken = spelerToken,
            });

            var spelInQueueResult = _mapper.Map<SpelDto>(query);

            return Ok(spelInQueueResult);
        }

        /// <summary>
        /// Action for updating/starting a 'open' spel entity to start the game.
        /// </summary>
        /// <param name="spelDto">Object for updating the spel entity which contains a SpelToken and a Speler2Token</param>
        /// <returns>IActionResult</returns>
        [HttpPut("participate")]
        public async Task<IActionResult> UpdateSpelInQueueParticipateAsync([FromBody] SpelParticipateDto spelDto)
        {
            if (spelDto is null)
            {
                _logger.LogError($"ParticipateInSpelDto sent from client is null.");
                return BadRequest("ParticipateInSpel object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError(
                    $"Invalid ParticipateSpelDto sent from client: {ModelState.Select(err => err.Value.Errors)}");
                return BadRequest("Invalid model object");
            }

            var command = await Mediator.Send(new ParticipateInSpelCommand
            {
                Token = (Guid)spelDto.SpelToken,
                Speler2Token = (Guid)spelDto.Speler2Token
            });

            return NoContent();
        }

        /// <summary>
        /// Action for all spel entities which are in process and thus playing.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>A PagedList with in process spel entities.</returns>
        [HttpGet]
        [Route("inprocess")]
        [QueryStringConstraint("spelerToken", false)]
        [QueryStringConstraint("", true)]
        public async Task<IActionResult> GetAllSpellenInProcessAsync([FromQuery] SpelParameters parameters)
        {
            var query = await Mediator.Send(new GetAllSpellenInProcessQuery
            {
                Params = parameters
            });

            var metadata = new
            {
                query.TotalCount,
                query.PageSize,
                query.CurrentPage,
                query.TotalPages,
                query.HasNext,
                query.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var spellenInProcess = _mapper.Map<List<SpelDto>>(query);

            return Ok(spellenInProcess);
        }

        /// <summary>
        /// Action for retrieving a specific in process spel based on its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An in process Spel entity.</returns>
        [HttpGet("inprocess/{id}")]
        public async Task<IActionResult> GetSpelInProcessByIdAsync(Guid id)
        {
            var query = await Mediator.Send(new GetSpelInProcessByIdQuery
            {
                Token = id
            });

            var spelInProcessResult = _mapper.Map<SpelDto>(query);

            return Ok(spelInProcessResult);
        }

        /// <summary>
        /// Action for retrieving a specific in process spel based on a spelerToken.
        /// </summary>
        /// <param name="spelerToken"></param>
        /// <returns>An in process Spel entity.</returns>
        [HttpGet]
        [Route("inprocess")]
        [QueryStringConstraint("spelerToken", true)]
        [QueryStringConstraint("", false)]
        public async Task<IActionResult> GetSpelInProcessBySpelerTokenAsync([FromQuery] Guid spelerToken)
        {
            var query = await Mediator.Send(new GetSpelInProcessBySpelerTokenQuery
            {
                SpelerToken = spelerToken
            });

            var spelInProcessResult = _mapper.Map<SpelDto>(query);

            return Ok(spelInProcessResult);
        }

        /// <summary>
        /// Action for updating an in process spel bord
        /// </summary>
        /// <param name="id"></param>
        /// <param name="spelMoveDto"></param>
        /// <returns>A BaseMoveModel containing a response for the client</returns>
        [HttpPut("inprocess/{id}/move")]
        public async Task<IActionResult> UpdateSpelInprocessMoveAsync([FromQuery] Guid id, [FromBody] SpelInProcessMoveDto spelMoveDto)
        {
            var command = await Mediator.Send(new InProcessSpelMoveCommand
            {
                HasPassed = spelMoveDto.HasPassed,
                X = spelMoveDto.X,
                Y = spelMoveDto.Y,
                Token = spelMoveDto.Token ?? id,
                SpelerToken = spelMoveDto.SpelerToken
            });

            var response = _mapper.Map(command, command.GetType(), typeof(BaseDto));

            return Ok(response);
        }

        /// <summary>
        /// Action for surrendering as a player 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surrenderDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut("inprocess/{id}/surrender")]
        public async Task<IActionResult> UpdateSpelInProcessSurrender(Guid id, [FromBody] SpelSurrenderDto surrenderDto)
        {
            //TODO: Request Body DTO
            var command = await Mediator.Send(new SurrenderSpelCommand
            {
                Token = id,
                SpelerToken = surrenderDto.SpelerToken ?? throw new Exception("SpelerToken was not in the request body")
            });

            return Ok(true);
        }

        /// <summary>
        /// Action for retrieving all finished spellen
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>A PagedList with all finished spel entities.</returns>
        [HttpGet]
        [Route("finished")]
        [QueryStringConstraint("spelerToken", false)]
        [QueryStringConstraint("", true)]
        public async Task<IActionResult> GetAllSpellenFinishedAsync([FromQuery] SpelParameters parameters)
        {
            var query = await Mediator.Send(new GetAllSpellenFinishedQuery
            {
                Paramameters = parameters
            });

            var metadata = new
            {
                query.TotalCount,
                query.PageSize,
                query.CurrentPage,
                query.TotalPages,
                query.HasNext,
                query.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var spellenFinished = _mapper.Map<List<SpelFinishedDto>>(query);

            return Ok(spellenFinished);
        }

        /// <summary>
        /// Action for getting a specific finished spel based on a token.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A finished Spel entity.</returns>
        [HttpGet("finished/{id}")]
        public async Task<IActionResult> GetSpelFinishedByIdAsync(Guid id)
        {
            var query = await Mediator.Send(new GetSpelFinishedByIdQuery
            {
                Token = id
            });

            var spelFinishedResult = _mapper.Map<SpelFinishedDto>(query);

            return Ok(spelFinishedResult);
        }

        /// <summary>
        /// Action for getting finished spel entities based on a certain spelertoken.
        /// </summary>
        /// <param name="spelerToken"></param>
        /// <param name="parameters"></param>
        /// <returns>A PageList with al finished spel entities based on a spelertoken.</returns>
        [HttpGet]
        [Route("finished")]
        [QueryStringConstraint("spelerToken", true)]
        [QueryStringConstraint("", false)]
        public async Task<IActionResult> GetSpelFinishedBySpelerTokenAsync([FromQuery] Guid spelerToken, [FromQuery] FinishedSpelParameters parameters)
        {
            var query = await Mediator.Send(new GetSpellenFinishedBySpelerTokenQuery
            {
                SpelerToken = spelerToken,
                Parameters = parameters,
            });

            var metadata = new
            {
                query.TotalCount,
                query.PageSize,
                query.CurrentPage,
                query.TotalPages,
                query.HasNext,
                query.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var spellenFinished = _mapper.Map<List<SpelFinishedDto>>(query);

            return Ok(spellenFinished);
        }

        /// <summary>
        /// Action for retrieving a single unfinished spel entity by spelertoken.
        /// </summary>
        /// <param name="spelerToken"></param>
        /// <returns>A single unfinished spel entity.</returns>
        [HttpGet]
        [Route("unfinished")]
        [QueryStringConstraint("spelerToken", true)]
        [QueryStringConstraint("", false)]
        public async Task<IActionResult> GetSpelUnFinishedBySpelerTokenAsync([FromQuery] Guid spelerToken)
        {
            var query = await Mediator.Send(new GetSpelUnFinishedBySpelerTokenQuery
            {
                SpelerToken = spelerToken,
            });

            var spelUnFinished = _mapper.Map<SpelDto>(query);

            return Ok(spelUnFinished);
        }

        [HttpDelete]
        [Route("unfinished/delete/{id}")]
        public async Task<IActionResult> DeleteSpelUnfinishedBySpelTokenAsync(Guid id)
        {
            var command = await Mediator.Send(new DeleteSpelUnFinishedBySpelTokenCommand
            {
                SpelToken = id,
            });

            return Ok(command);
        }
    }
}
