using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorControl.Api.Models;
using MotorControl.Api.Services;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MotorControl.Api.Controllers
{
    [Route("motors")]
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
        /// Search motors by availables and plate
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /motors?availables=true&plate=amv-20
        ///     
        /// </remarks>
        [ProducesResponseType(typeof(Response<List<MotorModelResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> Get([FromQuery] bool? availables, string? id, string? plate)
        {
            try
            {
                _logger.LogInformation($"Searching motors - {MethodBase.GetCurrentMethod()!.Name}");

                var motors = await Task.Run(() => _motorService.GetMotorsByAvailablesIdAndPlate(availables, id, plate));

                _logger.LogInformation($"Returning {motors.Count()} motors - {MethodBase.GetCurrentMethod()!.Name}");

                return Ok(motors);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// create - create a new register motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST 
        ///     {
        ///             "year": "2024",
        ///             "identifier": "3456745678",
        ///             "model": "Honda",
        ///             "plate": "amv-18"
        ///     }
        /// </remarks>
        /// <param name="motorModel"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create([FromBody] MotorModelRequest motorModel)
        {
            try
            {
                var plate = _motorService.GetMotorsByAvailablesIdAndPlate(plate: motorModel.Plate);
                if (!plate.Any())
                {
                    _logger.LogInformation($"Motor {plate} is already registered - {MethodBase.GetCurrentMethod()!.Name}");
                    return BadRequest("Motor plate exist");
                }

                await Task.Run(() => _motorService.Add(motorModel));
                _logger.LogInformation($"Adding motor - {MethodBase.GetCurrentMethod()!.Name}");

                var motorResponse = await Task.Run(() => _motorService.GetMotorsByAvailablesIdAndPlate(plate: motorModel.Plate!));
                return StatusCode(StatusCodes.Status201Created, motorResponse);

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
        ///     DELETE /plate
        ///    
        /// </remarks>
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status400BadRequest)]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete([FromQuery] string plate)
        {
            try
            {
                if (string.IsNullOrEmpty(plate))
                    return BadRequest("Plate is required");

                var plateExist = _motorService.GetMotorsByAvailablesIdAndPlate(plate: plate);

                if (!plateExist.Any())
                {
                    _logger.LogInformation($"Plate {plateExist} not found - {MethodBase.GetCurrentMethod()!.Name}");
                    return BadRequest($"Plate {plate} no found");
                }

                if (plateExist.FirstOrDefault()!.IsAvailable == 0)
                {
                    _logger.LogInformation($"Plate {plateExist.FirstOrDefault()!.Plate} cannot be removed because it is rented - {MethodBase.GetCurrentMethod()!.Name}");
                    return BadRequest($"Plate {plateExist.FirstOrDefault()!.Plate} cannot be removed because it is rented");
                }

                await Task.Run(() => _motorService.Delete(plate));
                _logger.LogInformation($"Deleting {plateExist} - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status204NoContent);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting motor {plate}- {MethodBase.GetCurrentMethod()!.Name}");

                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// update - update a register motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     PUT /update
        ///     {
        ///         "id": "string",
        ///         "isAvailable": true,
        ///         "plate": "amv-18"
        ///     }
        /// </remarks>
        /// <param name="motorModel"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<MotorModelResponse>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> Update([FromBody] MotorRequestUpdateModel motorModel)
        {
            try
            {
                var motor = await Task.Run(() => _motorService.GetMotorsByAvailablesIdAndPlate(id: motorModel.Id));
                if (motor != null)
                {
                    var exist = await Task.Run(() => _motorService.PlateBelongsToAnotherMotor(motorModel));
                    if (exist)
                        return BadRequest($"Plate {motorModel.Plate} belongs to another motorcycle");

                    var motorResponse = await Task.Run(() => _motorService.Update(motorModel));
                    
                    _logger.LogInformation($"Motor {motorResponse.Id} updated -  {MethodBase.GetCurrentMethod()!.Name}");
                    
                    return StatusCode(StatusCodes.Status202Accepted, motorResponse);

                }
                return BadRequest($"Motor {motorModel.Id} unavailable and can't do update");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }
    }
}
