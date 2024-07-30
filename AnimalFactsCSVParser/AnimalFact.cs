namespace AnimalFactsApi.Model;

public record AnimalFact(
    int Id,
    string AnimalName,
    string Source,
    string Text,
    string MediaLink,
    string WikiLink
    );