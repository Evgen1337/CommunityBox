using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.Web.Mvc.Clients.Gateway;
using CommunityBox.Web.Mvc.ViewModels.Auction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CommunityBox.Web.Mvc.Controllers
{
    [Authorize]
    [Route(Route)]
    public class AuctionController : Controller
    {
        public const string Route = "Auction";
        
        private readonly IGatewayClient _gatewayClient;
        private readonly IMapper _mapper;
        

        public AuctionController(IGatewayClient gatewayClient, IMapper mapper)
        {
            _gatewayClient = gatewayClient;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:long}")]
        public async Task<IActionResult> Index(long? id)
        {
            var userId = GetUserId();

            var responseModel = await _gatewayClient.GetAuctionAsync(id.Value);
            var retval = _mapper.Map<AuctionViewModel>(responseModel);
            retval.IsSubscribed = retval.Subscribers.Any(a => a.UserId == userId);
            retval.UserBet = retval.Auctioneers.FirstOrDefault(a => a.UserId == userId)?.Bet;

            return View(retval);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(NewAuctionViewModel viewModel)
        {
            var requestModel = _mapper.Map<NewAuctionContract>(viewModel);
            var responseModel = await _gatewayClient.CreateAuctionAsync(requestModel);

            return RedirectToAction("Index", new {id = responseModel.Id});
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("List/{userId}")]
        public async Task<IActionResult> List(string userId)
        {
            var responseModel = await _gatewayClient.GetAuctionsAsync(userId);
            var retval = _mapper.Map<IReadOnlyCollection<AuctionViewModel>>(responseModel);

            return View(retval);
        }

        [HttpGet]
        [Route("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            var responseModel = await _gatewayClient.GetAuctionAsync(id);
            var retval = _mapper.Map<UpdateAuctionViewModel>(responseModel);

            return View(retval);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Update/{id:long}")]
        public async Task<IActionResult> Update(UpdateAuctionViewModel viewModel, long id)
        {
            var requestModel = _mapper.Map<UpdateAuctionContract>(viewModel);
            var responseModel = await _gatewayClient.UpdateAuctionAsync(requestModel, id);

            return RedirectToAction("Index", new {id = responseModel.Id});
        }

        [HttpGet("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _gatewayClient.DeleteAuctionAsync(id);

            var userId = GetUserId();
            return RedirectToAction("List", new {userId});
        }

        [HttpPost("subscribe/{id:long}")]
        public async Task<IActionResult> Subscribe(long id)
        {
            var requestModel = new AddSubscriptionContract
            {
                AuctionId = id
            };

            await _gatewayClient.AddSubscriptionAsync(requestModel);

            return RedirectToAction("Index", new {id});
        }

        [HttpPost("unsubscribe/{id:long}")]
        public async Task<IActionResult> Unsubscribe(long id)
        {
            await _gatewayClient.RemoveSubscriptionAsync(id);

            return RedirectToAction("Index", new {id});
        }

        [HttpPost("setBet/{id:long}")]
        public async Task<IActionResult> SetBetAsync(long id, decimal betValue)
        {
            var requestModel = new SetBetContract
            {
                AuctionId = id,
                Value = betValue
            };

            await _gatewayClient.SetBetAsync(requestModel);

            return RedirectToAction("Index", new {id});
        }

        [HttpPost("removeBet/{id:long}")]
        public async Task<IActionResult> RemoveBetAsync(long id)
        {
            await _gatewayClient.RemoveBetAsync(id);
            return RedirectToAction("Index", new {id = id});
        }

        private string GetUserId() =>
            User.Claims.FirstOrDefault(f => f.Type == "uid")?.Value;
    }
}