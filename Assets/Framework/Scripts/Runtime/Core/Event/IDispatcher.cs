
namespace Cofdream.BaseFramework.Event
{
    public interface IDispatcher
    {
        void Subscribe<T>(short type, EventHandler<T> handler);
        void Unsubscribe<T>(short type, EventHandler<T> handler);
        void Subscribe(short type, EventHandler handler);
        void Unsubscribe(short type, EventHandler handler);
    }
}