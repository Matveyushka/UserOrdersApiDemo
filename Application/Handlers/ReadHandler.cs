using System.Collections.Generic;
using System.Linq;
using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public class ReadHandler<DataType, Output> : IMediatorHandler<object, ReadQuery<Output>, IEnumerable<Output>>
        where DataType : class, IEntity
    {
        private readonly IRepository<DataType> _repository;
        private readonly TypeMapper _typeMapper;
        private readonly ValueMapper _valueMapper;

        public ReadHandler(
            IRepository<DataType> repository, 
            TypeMapper typeMapper, 
            ValueMapper valueMapper)
        {
            this._repository = repository;
            this._typeMapper = typeMapper;
            this._valueMapper = valueMapper;
        }

        public IEnumerable<Output> Handle(ReadQuery<Output> request)
        {
            var data = _repository.Get();
            var dtoType = _typeMapper.ToDtoType<DataType>();
            var dtoData = data.Select(item => (Output)_valueMapper.Map(item, dtoType)).ToList();
            return (IEnumerable<Output>)dtoData;
        }
    }
}