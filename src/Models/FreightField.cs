namespace eShop.Api.Models;

public class FreightField
{
    public FreightField(int value, FreightFieldUnitMeasure unitMeasure)
    {
        if (value < 0)
            throw new ArgumentException();

        Value = value;
        UnitMeasure = unitMeasure;
    }
    public int Value { get; private set; }
    public FreightFieldUnitMeasure UnitMeasure { get; set; }
}