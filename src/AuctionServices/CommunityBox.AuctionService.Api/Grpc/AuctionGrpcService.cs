using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Commands;
using CommunityBox.AuctionService.Api.Application.Queries;
using CommunityBox.AuctionService.Api.Proto;
using Grpc.Core;
using MediatR;

namespace CommunityBox.AuctionService.Api.Grpc
{
    public class AuctionGrpcService : AuctionServices.AuctionServicesBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IAuctionQueries _auctionQueries;

        public AuctionGrpcService(IMediator mediator, IMapper mapper, IAuctionQueries auctionQueries)
        {
            _mediator = mediator;
            _mapper = mapper;
            _auctionQueries = auctionQueries;
        }

        public override async Task<CreateAuctionResponse> Create(CreateAuctionRequest request,
            ServerCallContext context)
        {
            var command = _mapper.Map<CreateAuctionCommand>(request);
            var retval = await _mediator.Send(command);
            var response = new CreateAuctionResponse
            {
                Auction = _mapper.Map<AuctionModel>(retval)
            };

            return response;
        }

        public override async Task<Empty> Delete(DeleteAuctionRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<DeleteAuctionCommand>(request);
            await _mediator.Send(command);

            return new Empty();
        }

        public override async Task<UpdateAuctionResponse> Update(UpdateAuctionRequest request,
            ServerCallContext context)
        {
            var command = _mapper.Map<UpdateAuctionCommand>(request);
            var retval = await _mediator.Send(command);
            var response = new UpdateAuctionResponse
            {
                Auction = _mapper.Map<AuctionModel>(retval)
            };

            return response;
        }

        public override async Task<GetAuctionResponse> Get(GetAuctionRequest request, ServerCallContext context)
        {
            var retval = await _auctionQueries.GetAuctionAsync(request.Id);
            var response = new GetAuctionResponse
            {
                Auction = _mapper.Map<AuctionModel>(retval)
            };

            return response;
        }

        public override async Task<GetAuctionListResponse> GetList(GetAuctionListRequest request,
            ServerCallContext context)
        {
            var retval = await _auctionQueries.GetAuctionListAsync(request.UserId);

            var response = new GetAuctionListResponse();
            response.Auctions.AddRange(_mapper.Map<IReadOnlyCollection<AuctionModel>>(retval));

            return response;
        }

        public override async Task<Empty> AddSubscriber(AddSubscriberRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<AddSubscriberCommand>(request);
            await _mediator.Send(command);

            return new Empty();
        }

        public override async Task<Empty> RemoveSubscriber(RemoveSubscriberRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<RemoveSubscriberCommand>(request);
            await _mediator.Send(command);

            return new Empty();
        }

        public override async Task<Empty> SetBet(SetBetRequest request, ServerCallContext context)
        {
            var command = new SetBetCommand(request.UserId, request.AuctionId, request.Value.ToDecimal());
            await _mediator.Send(command);

            return new Empty();
        }

        public override async Task<Empty> RemoveBet(RemoveBetRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<RemoveBetCommand>(request);
            await _mediator.Send(command);

            return new Empty();
        }
    }
}