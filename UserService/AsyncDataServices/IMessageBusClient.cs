using UserService.Dtos;

namespace UserService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishUserDeleted(UserDeletePublishedDto userDeletePublishedDto);
    }
}