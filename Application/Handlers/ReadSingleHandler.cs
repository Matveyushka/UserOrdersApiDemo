using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public class ReadSingleHandler<DataType, Output> : IMediatorHandler<int, ReadSingleQuery<Output>, Output>
        where DataType : class, IEntity
    {
        private readonly IRepository<DataType> _repository;
        private readonly TypeMapper _typeMapper;
        private readonly ValueMapper _valueMapper;

        public ReadSingleHandler(
            IRepository<DataType> repository, 
            TypeMapper typeMapper, 
            ValueMapper valueMapper)
        {
            this._repository = repository;
            this._typeMapper = typeMapper;
            this._valueMapper = valueMapper;
        }

        public Output Handle(ReadSingleQuery<Output> request)
        {
            var data = _repository.Find(request.Id);
            var dtoData = _valueMapper.Map(data, typeof(Output));
            return (Output)dtoData;
        }
    }
}