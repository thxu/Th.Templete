using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Th.Templete.Application.Demo;
using Th.Templete.Domain.Demo;

namespace Th.Templete.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IDemo _demo = null;

        public DemoController(IDemo demo)
        {
            _demo = demo;
        }

        [HttpGet]
        public async Task<DemoEntity> QueryDemoEntity(int id)
        {
            return await _demo.GetFirst(id);
        }
    }
}
