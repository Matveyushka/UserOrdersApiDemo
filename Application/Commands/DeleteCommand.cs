namespace UserOrdersApiDemo.Application
{
    public class DeleteCommand<DataType> : IMediatorRequest<DataType, CommandResponse>
    {
        public int Id { get; private set; }

        public DeleteCommand(int id)
        {
            this.Id = id;
        }
    }
}