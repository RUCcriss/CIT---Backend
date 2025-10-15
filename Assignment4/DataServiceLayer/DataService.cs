namespace DataServiceLayer;

public class DataService
{
    public string GetWelcomeMessage()
    {
        return "DataService fungerer korrekt!";
    }

    public List<string> GetTestData()
    {
        return new List<string>
        {
            "Test item 1",
            "Test item 2",
            "Test item 3"
        };
    }
}
