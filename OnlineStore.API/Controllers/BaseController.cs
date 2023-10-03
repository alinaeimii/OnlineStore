using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/store/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class BaseController : ControllerBase
{

}
