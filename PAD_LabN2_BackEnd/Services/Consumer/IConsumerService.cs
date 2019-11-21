using System.Collections.Generic;

namespace PAD_LabN2_BackEnd.Services.Consumer
{
    public interface IConsumerService
    {
        List<string> GetMessages();
    }
}