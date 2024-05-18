using Application.Order.Responses;
using Application.Production.Dto;
using MediatR;

namespace Application.Production.Commands
{
    public class ConcludedProductionCommand: IRequest<OrderResponse>
    {
        public ProductionDTO productionDTO { get; set; }
    }
}
