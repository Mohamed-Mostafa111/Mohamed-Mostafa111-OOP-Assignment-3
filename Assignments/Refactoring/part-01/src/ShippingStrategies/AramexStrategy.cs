using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part01.src.ShippingStrategies
{
	internal class AramexStrategy : IShappingStrategy
	{
		public decimal CalculateShippingCost(decimal weightKg)
		{
			return weightKg * 12m;
		}
	}
}
