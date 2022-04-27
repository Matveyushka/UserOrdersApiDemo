using System.Collections.Generic;

namespace UserOrdersApiDemo.Application
{
    public class ReadSingleQuery<Output> : IMediatorRequest<int, Output>
    {
        public int Id { get; private set; }

        public ReadSingleQuery(int id)
        {
            this.Id = id;
        }
    }
}