using Microsoft.AspNetCore.Mvc;
using UserOrdersApiDemo.Domain;
using UserOrdersApiDemo.Application;
using System.Collections.Generic;
using System.ComponentModel;

namespace UserOrdersApiDemo.WebApi
{
    [ApiController]
    public class EntityController<DataType, PostRequestType, PutRequestType, DtoType> : ControllerBase
        where DataType : class, IEntity, new()
    {
        Mediator _mediator;

        public EntityController(Mediator mediator)
        {
            this._mediator = mediator;
        }

        private IActionResult HandleCommandResponse(CommandResponse response) => response switch
        {
            CommandResponse.OK => Ok(),
            CommandResponse.FAIL => BadRequest(),
            CommandResponse.NOT_FOUND => NotFound(),
            _ => StatusCode(500)
        };

        [HttpGet("")]
        public IEnumerable<DtoType> Get() => _mediator
            .Send(new ReadQuery<DtoType>());

        [HttpGet("{id}")]
        public ActionResult<DtoType> Find(int id) => _mediator
            .Send(new ReadSingleQuery<DtoType>(id));

        [HttpPost("")]
        [Consumes("application/json")]
        public IActionResult Insert([FromBody] PostRequestType request) => HandleCommandResponse(
            _mediator
            .Send(new CreateCommand<PostRequestType>(request)) 
        );

        [HttpPut("")]
        [Consumes("application/json")]
        public IActionResult Update([FromBody] PutRequestType request) => HandleCommandResponse(
            _mediator
            .Send(new UpdateCommand<PutRequestType>(request)) 
        );

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => HandleCommandResponse(
            _mediator
            .Send(new DeleteCommand<DataType>(id))
        );
    }
}