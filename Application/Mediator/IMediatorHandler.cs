namespace UserOrdersApiDemo.Application
{
    public interface IMediatorHandler<Input, Request, Output>
        where Request : IMediatorRequest<Input, Output>
    {
        Output Handle(Request request);
    }   
}