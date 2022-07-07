// Example service to use for dependency injection in minimal api

namespace PizzaStore.Services.DependencyInjectionExampleService;

public interface IDependencyInjectionExampleService
{
    int GetRandomNumber();
}

public class DependencyInjectionExampleService : IDependencyInjectionExampleService
{
    private Random _Random;

    public DependencyInjectionExampleService()
    {
        _Random = new Random();
    }

    public int GetRandomNumber()
    {
        return _Random.Next();
    }
}