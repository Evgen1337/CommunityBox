using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.AuctionService.Api.Proto;
using CommunityBox.Api.WebGateway.Filters;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using CommunityBox.Common.AuthHelpers;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBox.Api.WebGateway.Controllers
{
    [ApiController]
    [Route(Route)]
    public class AuctionController : ControllerBase
    {
        private const string Route = "v1/auction";

        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public AuctionController(IAuctionService auctionService, IMapper mapper, JwtSecurityTokenHandler tokenHandler)
        {
            _auctionService = auctionService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [AuthorizeOnJwtSource]
        public async Task<IActionResult> CreateAsync([FromBody] NewAuctionContract content)
        {
            var requestModel = _mapper.Map<NewAuctionModel>(content);
            requestModel.OwnerUserId = GetUserId();

            var responseModel = await _auctionService.CreateAsync(requestModel);
            var retval = _mapper.Map<AuctionContract>(responseModel);
            return Ok(retval);
        }

        [AuthorizeOnJwtSource]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpdateAuctionContract content)
        {
            var requestModel = _mapper.Map<UpdateAuctionModel>(content);
            requestModel.OwnerUserId = GetUserId();
            requestModel.Id = id;

            var responseModel = await _auctionService.UpdateAsync(requestModel);
            var retval = _mapper.Map<AuctionContract>(responseModel);
            return Ok(retval);
        }

        [AuthorizeOnJwtSource]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _auctionService.DeleteAsync(id, GetUserId());
            return NoContent();
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var responseModel = await _auctionService.GetAsync(id);
            var retval = _mapper.Map<AuctionContract>(responseModel);
            return Ok(retval);
        }

        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetListAsync(string userId)
        {
            var responseModel = await _auctionService.GetListAsync(userId);
            var retval = _mapper.Map<IReadOnlyCollection<AuctionContract>>(responseModel);

            return Ok(retval);
        }

        [AuthorizeOnJwtSource]
        [HttpPost("subscription")]
        public async Task<IActionResult> AddSubscriptionAsync([FromBody] AddSubscriptionContract contract)
        {
            var userId = GetUserId();
            await _auctionService.AddSubscriberAsync(userId, contract.AuctionId);
            
            return Ok();
        }

        [AuthorizeOnJwtSource]
        [HttpDelete("subscription/{id:long}")]
        public async Task<IActionResult> RemoveSubscriptionAsync(long id)
        {
            var userId = GetUserId();
            await _auctionService.RemoveSubscriberAsync(userId, id);

            return NoContent();
        }

        [AuthorizeOnJwtSource]
        [HttpPost("bet")]
        public async Task<IActionResult> SetBetAsync([FromBody] SetBetContract contract)
        {
            var userId = GetUserId();
            await _auctionService.SetBetAsync(userId, contract.AuctionId, contract.Value);

            return Ok();
        }

        [AuthorizeOnJwtSource]
        [HttpDelete("bet/{id:long}")]
        public async Task<IActionResult> RemoveBetAsync(long id)
        {
            var userId = GetUserId();
            await _auctionService.RemoveBetAsync(userId, id);

            return NoContent();
        }

        private string GetUserId() =>
            Request.GetUserIdFromAuthorizationToken(_tokenHandler);
    }
}