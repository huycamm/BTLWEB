using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesStore.Models;
using ShoesStore.ViewModels;
using X.PagedList;

namespace ShoesStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        Qlbangiaynhom7Context db = new Qlbangiaynhom7Context();



    }
}
