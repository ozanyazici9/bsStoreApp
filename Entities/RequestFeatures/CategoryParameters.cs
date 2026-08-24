namespace Entities.RequestFeatures;

public class CategoryParameters : RequestParameters
{
    public string? SearchTerm { get; set; }

    public CategoryParameters()
    {
        OrderBy = "id";
    }
}
