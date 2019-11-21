using Microsoft.AspNetCore.Mvc;
using PAD_LabN2_BackEnd.Models;
using PAD_LabN2_BackEnd.Services;
using PAD_LabN2_BackEnd.Services.Consumer;

namespace PAD_LabN2_BackEnd.Controllers
{
    [ApiController]
    [Route("api")]
    public class MainController : ControllerBase
    {
        private static readonly string[] _dummyData = new[]
        {
            "Everything", "is", "working", "exactly", "as", "it", "should", "be", "!"
        };

        private IProducerService _producerService;
        private IConsumerService _consumerService; 

        public MainController
        (
            IProducerService producerService,
            IConsumerService consumerService
        )
        {
            _producerService = producerService;
            _consumerService = consumerService;
        }

        [HttpGet()]
        public IActionResult GetRoot()
        {
            return Ok("You are at the root of the project, congrats!\n" +
                      "\nWe have for you:" +
                      "\napi/dummy \t[Get] - Just for getting some data for testing." +
                      "\napi \t\t[Post] - For sending message by producer. Message: Json {body: string}.");
        }
        
        [HttpGet("dummy")]
        public IActionResult GetDummy()
        {
            return Ok(_dummyData);
        }
        
        [HttpGet("messages")]
        public IActionResult TryGetMessages()
        {
            return Ok(_consumerService.GetMessages());
        }

        [HttpPost]
        public IActionResult TryProduceMessage([FromBody]MessageRequest message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (_producerService.ProduceMessage(new Message()
            {
                TopicName = "my-replicated-topic",
                Body = message.Body
            }))
            {
                return Ok();
            }
            
            return BadRequest();
        }
        
    }
}