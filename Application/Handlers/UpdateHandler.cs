using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public class UpdateHandler<Input, DataType> : IMediatorHandler<Input, UpdateCommand<Input>, CommandResponse>
        where DataType : class, IEntity
    {
        private readonly IRepository<DataType> _repository;
        private readonly TypeMapper _typeMapper;
        private readonly ValueMapper _valueMapper;

        public UpdateHandler(
            IRepository<DataType> repository, 
            TypeMapper typeMapper, 
            ValueMapper valueMapper)
        {
            this._repository = repository;
            this._typeMapper = typeMapper;
            this._valueMapper = valueMapper;
        }

        public CommandResponse Handle(UpdateCommand<Input> request)
        {
            var entity = _valueMapper.Map<DataType>(request.Entity);

            if (_repository.Exists(entity.Id) == false)
            {
                return CommandResponse.NOT_FOUND;
            }
            try
            {
                _repository.Update(entity);
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