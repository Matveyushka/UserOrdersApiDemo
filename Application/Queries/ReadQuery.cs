using System.Collections.Generic;

namespace UserOrdersApiDemo.Application
{
    public class ReadQuery<Output> : IMediatorRequest<object, IEnumerable<Output>>
    {

    }
}