using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public class NullOrderProcessor : IOrderProcessor
    {
        public void ProcessOrder(Entities.Cart cart, Entities.ShippingDetails details)
        {
            //noop
        }
    }
}
