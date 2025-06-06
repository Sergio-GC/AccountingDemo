using BLLAccountingDemo;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace AccountingDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KidsController : ControllerBase
    {
        private readonly KidManager _kidManager;

        public KidsController(KidManager kidManager)
        {
            _kidManager = kidManager;
        }

        // GET: KidsController
        [HttpGet]
        public List<Kid> Index()
        {
            return _kidManager.GetKids();
        }

        // GET: KidsController/Details/5
        [HttpGet("Kid/{id}")]
        public Kid GetKid(int id)
        {
            return _kidManager.GetKid(id);
        }

        // Post: KidsController/Create
        [HttpPost]
        public void Create(Kid kid)
        {
            _kidManager.CreateKid(kid);
        }

        // PUT: KidsController/Update
        [HttpPut]
        public void Update(Kid kid)
        {
            _kidManager.UpdateKid(kid);
        }

        // POST: KidsController/Delete
        [HttpDelete]
        public void Delete(Kid kid, bool siblings)
        {
            _kidManager.RemoveKid(kid, siblings);
        }
    }
}
