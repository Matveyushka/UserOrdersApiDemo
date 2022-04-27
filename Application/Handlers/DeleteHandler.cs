using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public class DeleteHandler<DataType> : IMediatorHandler<DataType, DeleteCommand<DataType>, CommandResponse>
        where DataType : class, IEntity
    {
        private readonly IRepository<DataType> _repository;
        private readonly TypeMapper _typeMapper;
        private readonly ValueMapper _valueMapper;

        public DeleteHandler(
            IRepository<DataType> repository, 
            TypeMapper typeMapper, 
            ValueMapper valueMapper)
        {
            this._repository = repository;
            this._typeMapper = typeMapper;
            this._valueMapper = valueMapper;
        }

        public CommandResponse Handle(DeleteCommand<DataType> request)
        {
            if (this._repository.Exists(request.Id) == false)
            {
                return CommandResponse.NOT_FOUND;
            }

            this._repository.Delete(request.Id);
            this._repository.Save();
            return CommandResponse.OK;
        }
    }
}