using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApi1.Context;
using CoreApi1.IService;
using CoreApi1.Service;

namespace CoreApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICityService _iService;
        public ValuesController(ICityService iCityService)
        {
            this._iService = iCityService;
        }
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public async Task<object> Get()
        {
            var data = await this._iService.QueryAsync();
            return data;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("testdel")]
        public async Task<object> del(int id)
        {
            var data = await this._iService.Del(c => c.Id == id);
            return data;
        }

    }
}

