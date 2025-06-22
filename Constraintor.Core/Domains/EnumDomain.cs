namespace Constraintor.Core.Domains;

public class EnumDomain : SetDomain
{
    public Type EnumType { get; }

    public EnumDomain(Type enumType) : base(Enum.GetValues(enumType).Cast<object>().AsEnumerable())
    {
        if (!enumType.IsEnum)
            throw new ArgumentException("Provided type must be an enum.");

        EnumType = enumType;
    }

    public override string ToString() => $"EnumDomain: {EnumType.Name}";
}