using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part01.src.ShippingStrategies
{
	internal class FedExStrategy : IShappingStrategy
	{
		public decimal CalculateShippingCost(decimal weightKg)
		{
			return weightKg * 15m;
		}

	}
}
