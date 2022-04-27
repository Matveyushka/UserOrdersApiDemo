using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public class CreateHandler<Input, DataType> : IMediatorHandler<Input, CreateCommand<Input>, CommandResponse>
        where DataType : class, IEntity
    {
        private readonly IRepository<DataType> _repository;
        private readonly TypeMapper _typeMapper;
        private readonly ValueMapper _valueMapper;

        public CreateHandler(
            IRepository<DataType> repository,
            TypeMapper typeMapper,
            ValueMapper valueMapper)
        {
            this._repository = repository;
            this._typeMapper = typeMapper;
            this._valueMapper = valueMapper;
        }

        public CommandResponse Handle(CreateCommand<Input> request)
        {
            var entity = _valueMapper.Map<DataType>(request.Entity);

            try
            {
                _repository.Insert(entity);
                _repository.Save();
            }
            catch
            {
                return CommandResponse.FAIL;
            }

            return CommandResponse.OK;
        }
    }
}