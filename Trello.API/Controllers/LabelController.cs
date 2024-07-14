using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.Services.LabelServices;

namespace Trello.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {

        } 

    }
}
