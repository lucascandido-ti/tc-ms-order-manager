using Application.Order.Dto;
using Domain.Utils;

namespace Application.Order.Responses
{
    public class ListOrderResponse: Response
    {
        public List<OrderDTO> Data;
    }
}
