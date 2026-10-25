using RefactoringLab.Part01.src.ShippingStrategies;

namespace RefactoringLab;

public class ShippingCostCalculator
{
	public decimal Calculate(IShappingStrategy Strategy,decimal weightKg)
    {
        return Strategy.CalculateShippingCost(weightKg);
	}
}