namespace DataOptimizer.Models;

public class Country
{
    public Name Name { get; set; }
    public int Population { get; set; }

    public override bool Equals(object obj)
    {
        return obj is Country country &&
               EqualityComparer<Name>.Default.Equals(Name, country.Name) &&
               Population == country.Population;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Population);
    }
}

public class Name
{
    public string Common { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Common);
    }
}
