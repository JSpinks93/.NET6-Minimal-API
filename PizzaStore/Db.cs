// This class is housing models for our minimal api, and acting as a basic in memory DB

namespace PizzaStore.DB;

public record Pizza
{
    public int Id { get; set; }

    public string ?  Name { get; set; }
}

public class PizzaDB
{
    private static List<Pizza> _pizzas = new List<Pizza>()
    {
        new Pizza { Id = 1, Name = "Yuge Pizza, It's yuuuuge!"},
        new Pizza { Id = 2, Name = "Smol Pizza, It's smol."},
        new Pizza { Id = 3, Name = "Pizza Pizza, It's a Little Cesars pizza."}
    };

    public static List<Pizza> GetPizzas()
    {
        return _pizzas;
    }

    public static Pizza ? GetPizza(int id)
    {
        return _pizzas.SingleOrDefault(pizza => pizza.Id == id);
    }

    public static Pizza CreatePizza(Pizza pizza)
    {
        _pizzas.Add(pizza);
        return pizza;
    }

    public static Pizza UpdatePizza(Pizza updatedPizza)
    {
        _pizzas = _pizzas.Select(pizza =>
        {
            if (pizza.Id == updatedPizza.Id)
            {
                pizza.Name = updatedPizza.Name;
            }

            return pizza;
        }).ToList();

        return updatedPizza;
    }

    public static void RemovePizza(int id)
    {
        _pizzas = _pizzas.FindAll(pizza => pizza.Id != id).ToList();
    }
}