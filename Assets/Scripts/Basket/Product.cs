using System;

public struct Product : IEquatable<Product>
{
    public string Name { get; private set; }
    public float Price { get; private set; }
    public string Prefab { get; private set; }
    public string Material { get; private set; }

    public Product(string name, float price, string prefab, string material)
    {
        Name = name;
        Price = price;
        Prefab = prefab;
        Material = material;
    }

    public override bool Equals(object obj)
    {
        return obj is Product product && Equals(product);
    }

    public bool Equals(Product other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public static bool operator ==(Product left, Product right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Product left, Product right)
    {
        return !(left == right);
    }
}