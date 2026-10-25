using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part01.src.ShippingStrategies
{
	internal class DHLStrategy : IShappingStrategy
	{
		public decimal CalculateShippingCost(decimal weightKg)
		{
			return weightKg * 18m;
		}
	}
}
