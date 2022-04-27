namespace UserOrdersApiDemo.Application
{
    public class UpdateCommand<Input> : IMediatorRequest<Input, CommandResponse>
    {
        public Input Entity { get; private set; }

        public UpdateCommand(Input entity)
        {
            this.Entity = entity;
        }
    }
}