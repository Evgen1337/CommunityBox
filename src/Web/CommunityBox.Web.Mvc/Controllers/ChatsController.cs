using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.Web.Mvc.Clients.Chat;
using CommunityBox.Web.Mvc.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBox.Web.Mvc.Controllers
{
    [Authorize]
    public class ChatsController : Controller
    {
        private readonly IChatGrpcService _chatGrpcClient;
        private readonly IMapper _mapper;

        public ChatsController(IChatGrpcService chatGrpcClient, IMapper mapper)
        {
            _chatGrpcClient = chatGrpcClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var response = await _chatGrpcClient.GetChatsAsync(userId);
            var retval = _mapper.Map<IReadOnlyCollection<ChatPreviewViewModel>>(response);

            return View(retval);
        }
        
        [HttpPost]
        public async Task<IActionResult> SendSingleMessage(NewSingleMessageViewModel viewModel)
        {
            var userId = GetUserId();

            var requestModel = _mapper.Map<NewSingleMessageModel>(viewModel);
            requestModel.UserId = userId;
            
            await _chatGrpcClient.SendMessageAsync(requestModel);
            return Ok();
        }

        public string GetUserId() =>
            User.Claims.FirstOrDefault(f => f.Type == "uid")?.Value;
    }
}