using System.Runtime.CompilerServices;

namespace SrpLab;

/// <summary>
/// Warehouse pick list: naive path order, stock allocation, and picker instruction prose.
/// </summary>
public sealed class WarehousePickList
{
    private readonly List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines = new();
	private readonly PathingOrder pathingOrder ;
	private readonly StockAllocation stockAllocation;
	private readonly pickerinstruction pickerinstruction;


	public WarehousePickList()
	{
		pathingOrder = new PathingOrder(_lines);
		stockAllocation = new StockAllocation(_lines);
		pickerinstruction = new pickerinstruction(pathingOrder, stockAllocation, _lines);
	}

	public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand)
    {
        _lines.Add((sku, aisle, bin, qtyNeeded, qtyOnHand));
    }

	public string PickerScript()
	{
	  return pickerinstruction.PickerScript();
	}
}
public sealed class StockAllocation
{
    private readonly IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines;

	public StockAllocation(IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> lines)
	{
		_lines = lines;
	}
	public IReadOnlyList<(string Sku, int Allocated)> Allocate()
	{
		// Allocation/backorder policy ≠ walking path ≠ human instructions.
		var result = new List<(string, int)>();
		foreach (var line in _lines)
		{
			var alloc = Math.Min(line.QtyNeeded, line.QtyOnHand);
			result.Add((line.Sku, alloc));
		}
		return result;
	}
}

public sealed class PathingOrder
{
	private readonly IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines;

	public PathingOrder(IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> lines)
	{
		_lines = lines;
	}
	public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder()
	{
		// Path heuristic will change with warehouse layout tech.
		return _lines
			.OrderBy(l => l.Aisle)
			.ThenBy(l => l.Bin)
			.Select(l => (l.Aisle, l.Bin, l.Sku, Math.Min(l.QtyNeeded, l.QtyOnHand)))
			.Where(x => x.Item4 > 0)
			.ToList();
	}
}

public sealed class pickerinstruction
{
	private readonly PathingOrder order;
	private readonly StockAllocation stock;
	private readonly IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines;

	public pickerinstruction(
		PathingOrder order,
		StockAllocation stock,
		IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> lines)
	{
		this.order = order;
		this.stock = stock;
		_lines = lines;
	}

	public string PickerScript()
	{
		// UX wording for handheld devices — separate owners.
		var steps =order.WalkingOrder()
			.Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");
		var shortfalls =stock.Allocate().Where(a =>
		{
			var need = _lines.First(l => l.Sku == a.Sku).QtyNeeded;
			return a.Allocated < need;
		});
		var warn = shortfalls.Any()
			? "SHORTAGES: " + string.Join(", ", shortfalls.Select(s => s.Sku))
			: "SHORTAGES: none";
		return string.Join('\n', steps) + "\n" + warn;
	}
}

public sealed class WmsXmlExporter
{
	private readonly StockAllocation stock;

	public WmsXmlExporter(StockAllocation Stock)
	{
		stock = Stock;
	}

	public string WmsXmlBatch(string batchId)
	{
		// Integration contract with WMS — third reason to change.
		var parts =stock.Allocate().Select(a => $"<line sku=\"{a.Sku}\" qty=\"{a.Allocated}\" />");
		return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
	}
}