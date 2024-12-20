using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCamServer.Dtos.Notification;
using WebCamServer.Services.Interfaces;
//using WebCamServer.Models;

namespace WebCamServer.Controllers
{
  [Route("api/v1/notification")]
  [ApiController]
  public class NotificationController : ControllerBase
  {
    
    private string message_error = "Hubo un error, consulte con el administrador";
    private readonly IUserService _userServ;
    private readonly IAuthService _authServ;
    private readonly INotificationService _service;
    private readonly ILogger<NotificationController> _logger;
    public NotificationController(INotificationService service, ILogger<NotificationController> logger, IUserService userServ, IAuthService authServ)
    {
      _service = service;
      _logger = logger;
      _userServ = userServ;
      _authServ = authServ;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateNotification([FromBody] NotificationToCreateDto create)
    {
      try
      {
        var user_id = User.FindFirst("id")?.Value;
        if(user_id == null) Unauthorized("El usuario no es reconocido");
        int userId = Int32.Parse(user_id);

        create.UserId = userId;
        if(await _userServ.UserExist(create.UserId)) 
          return BadRequest("No existe el usuario");

        var response =  await _service.Create(create);

        if(response == null)
          return BadRequest("No se pudo crear");
        
        return Ok(response);
      }
      catch(Exception err)
      {
        _logger.LogError(err.Message);
        Console.WriteLine(err.StackTrace);
        return BadRequest(message_error);
      }
    }
    
    [HttpGet]
    public async Task<ActionResult> GetNotification()
    {
      try
      {
        var user_id = User.FindFirst("id")?.Value;
        if(user_id == null) Unauthorized("El usuario no es reconocido");
        int userId = Int32.Parse(user_id);

        var response =  await _service.GetList(userId);

        if(response.Count == 0)
          return Ok("No hay notificaciones");
          
        if(response == null)
          return BadRequest("Error al obtener notificaciones");
        
        return Ok(response);
      }
      catch(Exception err)
      {
        _logger.LogError(err.Message);
        Console.WriteLine(err.StackTrace);
        return BadRequest(message_error);
      }
    }
    
    [HttpGet("socket")]
    public async Task NotificationSocket()
    {
      try
      {
        Console.WriteLine("LO INTENTO EL WS");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
          // Extraer el token de los parámetros de consulta
          var token = HttpContext.Request.Headers["token"].ToString();
          var userId = _authServ.ValidateToken(token);
          
          if (userId == 0)
          {
            HttpContext.Response.StatusCode = 401; // No autorizado
            return;
          }

          WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
          await _service.NotificationConnection(webSocket, userId);
        }
        else
        {
          HttpContext.Response.StatusCode = 400;
        }
      }
      catch(Exception err)
      {
        _logger.LogError(err.Message);
        Console.WriteLine(err.StackTrace);
      }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateNotification([FromBody]  NotificationToUpdateDto update)
    {
      try
      {
        var response =  await _service.Update(update);
          
        if(response == null)
          return BadRequest("Error al actualizar notificacion");
        
        return Ok(response);
      }
      catch(Exception err)
      {
        _logger.LogError(err.Message);
        Console.WriteLine(err.StackTrace);
        return BadRequest(message_error);
      }
    }
  }
}