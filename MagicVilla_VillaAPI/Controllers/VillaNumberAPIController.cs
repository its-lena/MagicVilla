using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiVersion("1.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla,IMapper mapper)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
           // _logger = logger;
           _dbVilla = dbVilla;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasNumber()
        {
            try
            {
                IEnumerable<VillaNumber> VillaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(VillaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name ="CreateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var villa = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, includeProperties:"Villa");
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string>() { ex.Message};
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (villaCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == villaCreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number must be unique..");
                    return BadRequest(ModelState);
                }

                if (await _dbVilla.GetAsync(u => u.Id == villaCreateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Invalid Villa ID");
                    return BadRequest(ModelState);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaCreateDTO);
                await _dbVillaNumber.CreateAsync(villaNumber);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                return CreatedAtRoute("CreateVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                VillaNumber villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber (int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (id != villaNumberUpdateDTO.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (await _dbVilla.GetAsync(u => u.Id == villaNumberUpdateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Invalid Villa ID");
                    return BadRequest(ModelState);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                await _dbVillaNumber.UpdateAsync(villaNumber);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=
                    new List<string>() { ex.Message };
            }
            return _response;
        }

    }
}
