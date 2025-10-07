namespace Data.Goods
{
    public record Recipe(Good Result, Good[] Ingredients = null);
}