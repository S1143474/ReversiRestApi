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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Application.Spellen.Commands.CreateSPel;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Application.Spellen.Queries.GetSpellen;

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

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var spelResult = _mapper.Map<List<SpelDto>>(query);

            return Ok(spelResult);
        }

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


        /*[HttpGet("index")]
        public ActionResult<string> Index()
        {
            return "Hoi";
        }

        [HttpGet]
        public async Task<ActionResult<IList<SpelDTO>>> GetReversiListWithWaitingSpelers(CancellationToken token)
        {
            var result = await Mediator.Send(new LoadAllReversiWithWaitingSpelersQuery(), token);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PlaceReversi([FromBody] CreateReversiCommand contract, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(contract, cancellationToken);
            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SpelDTO>> GetReversiById(string id, CancellationToken token)
        {
            var result = await Mediator.Send(new LoadReversiById
            {
                SpelToken = id
            });

            return result.FirstOrDefault();
        }

        [HttpGet("SpelSpeler/{spelerToken}")]
        public async Task<ActionResult<SpelDTO>> GetSpelFromSpelerToken(string spelerToken, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new LoadReversiBySpelerToken
            {
                SpelerToken = spelerToken,
            }, cancellationToken);

            return result;
        }

        [HttpPut("join")]
        public async Task<ActionResult<bool>> JoinGameReversi([FromBody] JoinReversiCommand contract, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(contract, cancellationToken);

            return result;
        }

        [HttpGet("SpelToken")]
        public async Task<ActionResult<string>> GetSpelTokenFromSpelerToken(string spelerToken, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new LoadReversiBySpelerToken
            {
                SpelerToken = spelerToken
            }, cancellationToken);

            if (result == null)
                return BadRequest();

            return result.Token;
        }

        [HttpPut("Zet")]
        public async Task<ActionResult<BaseDTO>> PutDoMove(CancellationToken cancellationToken,
            [FromBody] DoMoveCommand contract)
        {
            var result = await Mediator.Send(contract, cancellationToken);
            return result;
        }

        [HttpGet("SpelFinished")]
        public async Task<ActionResult<SpelReultsDTO>> GetSpelFinishedResults(string spelToken, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new LoadReversiFinishedResultsQuery
            {
                SpelToken = spelToken
            }, cancellationToken);


            var isFinished = await Mediator.Send(new FinishSpelCommand
            {
                SpelToken = spelToken
            }, cancellationToken);

            return result;
        }*/

    }
}
