using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.IdentityService.Api.Domain.Entities;
using CommunityBox.IdentityService.Api.Proto;
using CommunityBox.IdentityService.Api.Services;
using CommunityBox.IdentityService.AuthService.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CommunityBox.IdentityService.Api.Grpc
{
    public class IdentityGrpcServices : IdentityServices.IdentityServicesBase
    {
        private readonly IMapper _mapper;
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenManager _jwtTokenManager;

        public IdentityGrpcServices(IMapper mapper, IdentityUserManager identityUserManager,
            SignInManager<User> signInManager, IJwtTokenManager jwtTokenManager)
        {
            _mapper = mapper;
            _userManager = identityUserManager;
            _signInManager = signInManager;
            _jwtTokenManager = jwtTokenManager;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            if (await _userManager.IsExistByEmailAsync(request.User.Email))
                throw new RpcException(
                    new Status(StatusCode.AlreadyExists, "Пользователь с таким email уже существует"));

            if (await _userManager.IsExistByPhoneAsync(request.User.PhoneNumber))
                throw new RpcException(new Status(StatusCode.AlreadyExists,
                    "Пользователь с таким телефоном уже существует"));

            var user = _mapper.Map<User>(request.User);
            user.AccountSetting = new AccountSetting();
            user.CreationUtcDate = DateTime.UtcNow;

            var createdResult = await _userManager.CreateAsync(user, request.User.Password);

            if (createdResult.Succeeded)
                return new CreateUserResponse
                {
                    User = _mapper.Map<UserModel>(user)
                };

            var detail = string.Join(" ,", createdResult.Errors.Select(s => s.Description));
            throw new RpcException(new Status(StatusCode.Cancelled, detail));
        }

        [Authorize]
        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var userId = GetUserId(context);

            if (request.User.Id != userId)
                throw new RpcException(
                    new Status(StatusCode.Unauthenticated, "Недостаточно прав"));

            var user = await _userManager.FindFullInfoUserByIdAsync(request.User.Id);

            if (user == null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, "Пользователь не найден"));

            user.UserPersonalInformation.FirstName = request.User.UserPersonalInformation.FirstName;
            user.UserPersonalInformation.LastName = request.User.UserPersonalInformation.LastName;
            user.UserPersonalInformation.Bio = request.User.UserPersonalInformation.Bio;
            user.UserPersonalInformation.BirthDay = request.User.UserPersonalInformation.BirthDay.ToDateTime();

            user.AccountSetting.ShowPhone = request.User.AccountSetting.ShowPhone;
            user.AccountSetting.ShowEmail = request.User.AccountSetting.ShowEmail;
            user.AccountSetting.ShowBirthDay = request.User.AccountSetting.ShowBirthDay;

            user.UserName = request.User.UserName;
            user.UpdateUtcDate = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
                return new UpdateUserResponse
                {
                    User = _mapper.Map<UserModel>(user)
                };

            var detail = string.Join(" ,", updateResult.Errors.Select(s => s.Description));
            var status = new Status(StatusCode.Cancelled, detail);
            throw new RpcException(status);
        }

        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindFullInfoUserByIdAsync(request.Id);

            if (user == null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, "Пользователь не найден"));

            return new GetUserResponse
            {
                User = _mapper.Map<UserModel>(user)
            };
        }
        
        public override async Task<AuthResponse> Auth(AuthRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByEmailAsync(request.LogInModel.Email);

            if (user is null)
                throw new RpcException(new Status(StatusCode.Cancelled, "Неверный логин или пароль"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LogInModel.Password, false);

            if (!result.Succeeded)
                throw new RpcException(new Status(StatusCode.Cancelled, "Неверный логин или пароль"));

            var token = _jwtTokenManager.GenerateToken(user.Email, user.Id);

            return new AuthResponse
            {
                Auth = new AuthModel
                {
                    Email = user.Email,
                    UserId = user.Id,
                    Token = token
                }
            };
        }
        
        public override Task<ValidateTokenResponse> ValidateToken(ValidateTokenRequest request,
            ServerCallContext context)
        {
            var isValid = _jwtTokenManager.ValidateToken(request.Token);
            
            return Task.FromResult(new ValidateTokenResponse
            {
                IsValid = isValid
            });
        }

        private string GetUserId(ServerCallContext context) =>
            _jwtTokenManager.GetUserIdFromHeaderToken(context.GetHttpContext().Request);
    }
}