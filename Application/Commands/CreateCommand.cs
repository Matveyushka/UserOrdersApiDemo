namespace UserOrdersApiDemo.Application
{
    public class CreateCommand<Input> : IMediatorRequest<Input, CommandResponse>
    {
        public Input Entity { get; private set; }

        public CreateCommand(Input entity)
        {
            this.Entity = entity;
        }
    }
}