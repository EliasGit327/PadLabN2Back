using PAD_LabN2_BackEnd.Models;

namespace PAD_LabN2_BackEnd.Services
{
    public interface IProducerService
    {
        bool ProduceMessage(Message message);
    }
}