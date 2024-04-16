using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorControl.Api.Models;
using MotorControl.Api.Services;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MotorControl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorController : ControllerBase
    {
        private readonly IMotorService _motorService;
        private readonly ILogger<MotorController> _logger;

        public MotorController(IMotorService motorService, ILogger<MotorController> logger)
        {
            _motorService = motorService;
            _logger = logger;
        }

        /// <summary>
        /// motors - Search all registered motors
        /// </summary>
        [ProducesResponseType(typeof(Response<ShowMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ShowMotorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ShowMotorModel>), StatusCodes.Status500InternalServerError)]
        [HttpGet("motors")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Searching all motors - {MethodBase.GetCurrentMethod()!.Name}");

                var motors = await Task.Run(() => _motorService.Get());
                if (motors is not null)
                {
                    _logger.LogInformation($"Returning {motors.Count()} motors - {MethodBase.GetCurrentMethod()!.Name}");

                    return Ok(motors);
                }
                _logger.LogError($"Returning {motors?.Count()} motors - {MethodBase.GetCurrentMethod()!.Name}");

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// motorByPlate - Search registered motor by Id
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /motorByPlate?Id={{plate}}
        ///     
        /// </remarks>
        /// <param name="Id">Id do usuario</param>
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status400BadRequest)]
        [HttpGet("motorByPLate")]
        public async Task<ActionResult> GetByPlate(string plate)
        {
            try
            {
                if (string.IsNullOrEmpty(plate))
                    return BadRequest();

                var motor = await Task.Run(() => _motorService.GetByPlate(plate));

                return Ok(motor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// create - create motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /create
        ///     {
        ///         "id": "12346789",
        ///         "name": "Pepe",
        ///         "secondName": "Ostias",
        ///         "email": "pepe@example.com",
        ///         "phoneNumber": "34234567",
        ///         "cpfCnpj": "222.222.222-22",
        ///         "address": {
        ///                     "id": "12346789",
        ///                     "street": "Calle1",
        ///                     "city": "São Paulo",
        ///                     "state": "São Paulo",
        ///                     "postalCode": "02700-000",
        ///                     "country": "Brasil",
        ///                     "number": "10",
        ///                     "complement": "casa"
        ///          }
        ///      }
        ///     
        /// </remarks>
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] MotorModel motorModel)
        {
            try
            {
                var plate = _motorService.GetByPlate(motorModel.MotorPlate)?.MotorPlate;
                if (string.IsNullOrEmpty(plate))
                {
                    _logger.LogInformation($"Adding motor - {MethodBase.GetCurrentMethod()!.Name}");
                    await Task.Run(() => _motorService.Add(motorModel));
                    return StatusCode(StatusCodes.Status201Created);
                }
                return BadRequest("Motor plate exist");

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// delete - delete motor by Plate
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     DELETE /delete?Plate={{Plate}}
        ///    
        /// </remarks>
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status400BadRequest)]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string plate)
        {
            try
            {
                if (string.IsNullOrEmpty(plate))
                    return BadRequest("Plate Required");

                var exist = _motorService.GetByPlate(plate)?.MotorPlate ;

                if (!string.IsNullOrEmpty(exist))
                {
                    _logger.LogInformation($"Deleting motor {plate}- {MethodBase.GetCurrentMethod()!.Name}");

                    await Task.Run(() => _motorService.Delete(plate));

                    return Ok(true);
                }
                return BadRequest("Plate no exist");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting motor {plate}- {MethodBase.GetCurrentMethod()!.Name}");

                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// update - update motor registered
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /update
        ///     {
        ///         "id": "12346789",
        ///         "name": "Pepe",
        ///         "secondName": "Ostias",
        ///         "email": "pepe@example.com",
        ///         "phoneNumber": "34234567",
        ///         "cpfCnpj": "222.222.222-22",
        ///         "address": {
        ///                     "id": "12346789",
        ///                     "street": "Calle1",
        ///                     "city": "São Paulo",
        ///                     "state": "São Paulo",
        ///                     "postalCode": "02700-000",
        ///                     "country": "Brasil",
        ///                     "number": "10",
        ///                     "complement": "casa"
        ///          }
        ///      }
        ///     
        /// </remarks>
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModel>), StatusCodes.Status400BadRequest)]
        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] ShowMotorModel motorModel)
        {
            try
            {
                var plate = await Task.Run(() => _motorService.GetByPlate(motorModel.MotorPlate));
                if (!string.IsNullOrEmpty(plate?.MotorPlate))
                {
                    return BadRequest($"Motor plate {plate.MotorPlate} exist can't do update");
                }

                var flag = await Task.Run(() => _motorService.Update(motorModel));
                
                if (flag) 
                {
                    _logger.LogInformation($"Motor updated :{motorModel.MotorPlate}  -  {MethodBase.GetCurrentMethod()!.Name}");
                    return Ok(true);

                }

                return BadRequest($"Id Motor {motorModel.Id} no exist");

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");

                return BadRequest(ex.Message);
            }
        }
    }
}
