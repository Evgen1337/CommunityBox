using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.Api.WebContracts.Identities;
using CommunityBox.Api.WebGateway.Filters;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using CommunityBox.Common.AuthHelpers;
using CommunityBox.IdentityService.Api.Proto;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBox.Api.WebGateway.Controllers
{
    [ApiController]
    [Route(Route)]
    public class IdentityController : ControllerBase
    {
        private const string Route = "v1/identity";

        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public IdentityController(IIdentityService identityService, IMapper mapper, JwtSecurityTokenHandler tokenHandler)
        {
            _identityService = identityService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
        }
        
        [HttpPost("auth")]
        public async Task<IActionResult> AuthAsync([FromBody] LogInContract content)
        {
            var requestModel = _mapper.Map<LogInModel>(content);
            var responseModel = await _identityService.AuthAsync(requestModel);
            var retval = _mapper.Map<AuthContract>(responseModel);

            return Ok(retval);
        }

        [AuthorizeOnJwtSource]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateTokenAsync()
        {
            var responseModel = await _identityService.ValidateTokenAsync(Request);

            return responseModel
                ? Ok()
                : BadRequest();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var responseModel = await _identityService.GetUserAsync(id);
            var retval = _mapper.Map<UserContract>(responseModel);

            return Ok(retval);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] NewUserContract content)
        {
            var requestModel = _mapper.Map<NewUserModel>(content);
            var responseModel = await _identityService.CreateUserAsync(requestModel);
            var retval = _mapper.Map<UserContract>(responseModel);

            return Ok(retval);
        }

        [HttpPut]
        [AuthorizeOnJwtSource]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserContract content)
        {
            var requestModel = _mapper.Map<UpdateUserModel>(content);
            requestModel.Id = Request.GetUserIdFromAuthorizationToken(_tokenHandler);

            var responseModel = await _identityService.UpdateUserAsync(requestModel, Request);
            var retval = _mapper.Map<UserContract>(responseModel);
            
            return Ok(retval);
        }
    }
}