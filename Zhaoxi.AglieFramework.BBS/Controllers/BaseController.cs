using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc; 

namespace Zhaoxi.AglieFramework.BBS.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	[EnableCors("any")]
	public class BaseController : ControllerBase
	{

	}
}