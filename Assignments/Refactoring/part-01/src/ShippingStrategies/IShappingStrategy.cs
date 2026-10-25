using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part01.src.ShippingStrategies
{
	public interface IShappingStrategy
	{
		decimal CalculateShippingCost(decimal weightKg);
	}
}
