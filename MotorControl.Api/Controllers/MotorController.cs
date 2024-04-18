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
        /// motors - Search all motors registered
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /motors
        ///     
        /// </remarks>
        /// <param name="plate">motor plate</param>
        [ProducesResponseType(typeof(Response<MotorModelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModelResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<MotorModelResponse>), StatusCodes.Status500InternalServerError)]
        [HttpGet("motors")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Searching all motors - {MethodBase.GetCurrentMethod()!.Name}");

                var motors = await Task.Run(() => _motorService.Get());
                if (motors.Any())
                {
                    _logger.LogInformation($"Returning {motors.Count()} motors - {MethodBase.GetCurrentMethod()!.Name}");

                    return Ok(motors);
                }
                _logger.LogInformation($"Motors not found - {MethodBase.GetCurrentMethod()!.Name}");

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// motor-plate - Search motor by plate
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /motor-plate?plate={{plate}}
        ///     
        /// </remarks>
        /// <param name="plate">motor plate</param>
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status404NotFound)]
        [HttpGet("motor-plate")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetByPlate(string plate)
        {
            try
            {
                if (string.IsNullOrEmpty(plate))
                    return BadRequest($"Plate is required");

                _logger.LogInformation($"Searching motor by plate - {MethodBase.GetCurrentMethod()!.Name}");

                var motor = await Task.Run(() => _motorService.GetByPlate(plate));
                if (motor != null)
                {
                    _logger.LogInformation($"Returning motor {motor.MotorPlate} - {MethodBase.GetCurrentMethod()!.Name}");

                    return Ok(motor);
                }

                _logger.LogInformation($"Motor {motor?.MotorPlate} not found- {MethodBase.GetCurrentMethod()!.Name}");

                return NotFound();
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
        ///     POST /create
        ///     {
        ///             "modelYear": "2024",
        ///             "model": "Honda",
        ///             "motorPlate": "amv-18"
        ///     }
        /// </remarks>
        /// <param name="motorModel"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create([FromBody] MotorModelRequest motorModel)
        {
            try
            {
                var plate = _motorService.GetByPlate(motorModel.MotorPlate)?.MotorPlate;
                if (!string.IsNullOrEmpty(plate))
                {
                    _logger.LogInformation($"Motor {plate} is already registered - {MethodBase.GetCurrentMethod()!.Name}");
                    return BadRequest("Motor plate exist");
                }

                await Task.Run(() => _motorService.Add(motorModel));
                _logger.LogInformation($"Adding motor - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created);

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
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status400BadRequest)]
        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string plate)
        {
            try
            {
                if (string.IsNullOrEmpty(plate))
                    return BadRequest("Plate is required");

                var plateExist = _motorService.GetByPlate(plate)?.MotorPlate;

                if (string.IsNullOrEmpty(plateExist))
                {
                    _logger.LogInformation($"Plate {plateExist} not found - {MethodBase.GetCurrentMethod()!.Name}");
                    return BadRequest($"Plate {plate} no found");
                }

                await Task.Run(() => _motorService.Delete(plate));
                _logger.LogInformation($"Deleting {plateExist} - {MethodBase.GetCurrentMethod()!.Name}");

                return Ok(true);
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
        ///         "modelYear": "2024",
        ///         "model": "Honda",
        ///         "motorPlate": "amv-18"
        ///     }
        /// </remarks>
        /// <param name="motorModel"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<MotorModelRequest>), StatusCodes.Status400BadRequest)]
        [HttpPut("update")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> Update([FromBody] MotorRequestUpdateModel motorModel)
        {
            try
            {
                var motor = await Task.Run(() => _motorService.GetById(motorModel.Id));
                if (motor != null)
                {
                    var flag = await Task.Run(() => _motorService.Update(motorModel));

                    if (flag)
                    {
                        _logger.LogInformation($"Motor {motorModel.MotorPlate} updated -  {MethodBase.GetCurrentMethod()!.Name}");
                        return Ok(true);
                    }
                    return Ok(false);
                }
                return BadRequest($"Motor plate {motorModel.Id} not exist and can't do update");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }
    }
}
