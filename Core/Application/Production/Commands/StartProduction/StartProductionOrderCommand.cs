using Application.Order.Responses;
using Application.Production.Dto;
using MediatR;

namespace Application.Production.Commands.StartProduction
{
    public class StartProductionOrderCommand : IRequest<OrderResponse>
    {
        public ProductionDTO productionDTO { get; set; }
    }
}
