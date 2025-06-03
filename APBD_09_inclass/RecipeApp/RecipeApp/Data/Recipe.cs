namespace RecipeApp;

public class Recipe
{
    public int RecipeId { get; set; }
    public required string Name { get; set; }
    public TimeSpan TimeToCook { get; set; }
    public bool isDeleted { get; set; }
    public bool isVegan { get; set; }
    public bool isVegetarian { get; set; }
    public required string Method { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}