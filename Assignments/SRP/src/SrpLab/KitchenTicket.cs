namespace SrpLab;

/// <summary>
/// Kitchen ticket: allergen scan from ingredients, cook ETA heuristics, and printed ticket layout.
/// </summary>
public sealed class KitchenTicket
{
    private readonly List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();

	private readonly AllergenScanner allergenScanner;


	private readonly KitchenOperations kitchenOperations;
	private readonly ThermalPrinterFormatter thermalPrinterFormatter;
	public KitchenTicket()
	{
		allergenScanner = new AllergenScanner(_items);
		kitchenOperations = new KitchenOperations(_items, allergenScanner);
		thermalPrinterFormatter = new ThermalPrinterFormatter(_items, allergenScanner,kitchenOperations);
	}

	public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add((item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }
	public string RenderThermalTicket(int OrderNumber)
	{
	 return thermalPrinterFormatter.RenderThermalTicket(OrderNumber);
	}
}

public sealed class AllergenScanner
{
    private readonly IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> _items;
    public AllergenScanner(IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> items)
	{
		_items = items;
	}
	public IReadOnlyList<string> DetectAllergens()
	{
		// Regulatory allergen dictionary changes separately from ticket layout.
		var hits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (var (_, ingredients, _) in _items)
		{
			foreach (var ing in ingredients)
			{
				if (ing.Contains("milk") || ing.Contains("cheese") || ing.Contains("butter")) hits.Add("dairy");
				if (ing.Contains("wheat") || ing.Contains("flour") || ing.Contains("bread")) hits.Add("gluten");
				if (ing.Contains("peanut") || ing.Contains("almond") || ing.Contains("cashew")) hits.Add("nuts");
				if (ing.Contains("shrimp") || ing.Contains("prawn") || ing.Contains("crab")) hits.Add("shellfish");
			}
		}
		return hits.OrderBy(x => x).ToList();
	}
}

public sealed class KitchenOperations
{
	private readonly IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> _items;
	private readonly AllergenScanner allergenScanner ;
	public KitchenOperations(IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> items,AllergenScanner allergenScanner)
	{
		_items = items;
		this.allergenScanner = allergenScanner;
	}
	public int EstimatedReadyMinutes(int openStations)
	{

		// Kitchen ops model ≠ printing.
		if (openStations <= 0) openStations = 1;
		var sequential = _items.Sum(i => i.PrepMinutes);
		var parallel = (int)Math.Ceiling(sequential / (double)openStations);
		if (allergenScanner.DetectAllergens().Count > 0) parallel += 3; // allergy protocol delay mixed in
		var longest = _items.Count == 0 ? 0 : _items.Max(i => i.PrepMinutes);
		return Math.Max(parallel, longest);
	}
}

public sealed class ThermalPrinterFormatter
{
	private readonly AllergenScanner allergenScanner;
	private readonly KitchenOperations kitchenOperations;


	private readonly IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> _items;
	public ThermalPrinterFormatter(IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> items, AllergenScanner allergenScanner, KitchenOperations kitchenOperations)
	{
		_items = items;
		this.allergenScanner = allergenScanner;
		this.kitchenOperations = kitchenOperations;
	}

	public string RenderThermalTicket(int orderNumber )
	{
		// Hardware/formatting concerns — width, separators — change with printer vendor.
		var width = 32;
		var line = new string('=', width);
		var body = string.Join('\n', _items.Select(i => $"* {i.Item.ToUpperInvariant()} ({i.PrepMinutes}m)"));
		var allergens = allergenScanner.DetectAllergens();
		var allergyLine = allergens.Count == 0 ? "ALLERGENS: none" : "ALLERGENS: " + string.Join(",", allergens);
		return $"{line}\nORDER #{orderNumber}\nETA {kitchenOperations.EstimatedReadyMinutes(2)} MIN\n{body}\n{allergyLine}\n{line}\n";
	}

	public string ExpoLaneHint()
	{
		return allergenScanner.DetectAllergens().Count > 0 ? "LANE-ALLERGY" : kitchenOperations.EstimatedReadyMinutes(2) > 20 ? "LANE-SLOW" : "LANE-FAST";
	}
}
