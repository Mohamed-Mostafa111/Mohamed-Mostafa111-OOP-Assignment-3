namespace SrpLab;

/// <summary>
/// Online checkout basket: totals, coupon linguistics, gift-wrap copy, and a faux payment auth code.
/// </summary>
public sealed class CheckoutBasket
{
    private readonly List<(string Sku, decimal Price, int Qty)> _lines = new();
	private readonly CouponManagement couponManagement;
	private readonly PaymentManagemet payment;
	public CheckoutBasket()
	{
		couponManagement = new CouponManagement(_lines);
		payment = new PaymentManagemet(_lines);
	}

	public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add((sku, price, qty));
    }

	public void ApplyCouponText(string? couponText)
	{
		couponManagement.ApplyCouponText(couponText);
	}
	public void EnableGiftWrap()
	{
		payment.EnableGiftWrap();
	}
	public decimal GrandTotal() => payment.GrandTotal();
	public string AuthorizePaymentStub(string cardLast4) => payment.AuthorizePaymentStub(cardLast4);
}

public sealed class MessagingSystem
{
    private readonly IReadOnlyList<(string Sku, decimal Price, int Qty)> _lines;

	public MessagingSystem (IReadOnlyList<(string Sku, decimal Price, int Qty)> values)
	{
		_lines = values;
	}
	public string GiftMessageCard(string fromName, decimal grandTotal)
	{
		// Customer-facing copy will change with marketing, not with totals.
		var items = string.Join(", ", _lines.Select(l => l.Sku));
		return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {grandTotal:C}\n";// Refactoring
    }
}

public sealed class CouponManagement
{
	private string? _couponRaw;
	private readonly IReadOnlyList<(string Sku, decimal Price, int Qty)> _lines;
	public CouponManagement(IReadOnlyList<(string Sku, decimal Price, int Qty)> values)
	{
		_lines = values;
	}

	public void ApplyCouponText(string? couponText) => _couponRaw = couponText;

	public decimal DiscountAmount(decimal subTotal)
	{
		// Parsing marketing strings is a different reason to change than pricing math.
		if (string.IsNullOrWhiteSpace(_couponRaw)) return 0m;
		var t = _couponRaw.Trim().ToUpperInvariant();
		if (t.StartsWith("SAVE") && int.TryParse(t[4..], out var pct) && pct is > 0 and <= 50)
			return Math.Round(subTotal * pct / 100m, 2);
		if (t.Contains("FREESHIP")) return 0m; // ship is elsewhere — still parsed here
		if (t == "WELCOME10") return Math.Min(10m, subTotal);
		return 0m;
	}

}

public sealed class PaymentManagemet
{
	private bool _giftWrap;

	private readonly CouponManagement couponManagement;
	private readonly IReadOnlyList<(string Sku, decimal Price, int Qty)> _lines;
	public PaymentManagemet(IReadOnlyList<(string Sku, decimal Price, int Qty)> lines)
	{
		_lines = lines;
		couponManagement = new CouponManagement(lines);
	}

	public decimal SubTotal() => _lines.Sum(l => l.Price * l.Qty);
	public void EnableGiftWrap() => _giftWrap = true;

	public decimal GrandTotal()
	{
		var total = SubTotal() - couponManagement.DiscountAmount(SubTotal());
		if (_giftWrap) total += 4.99m; // packaging fee policy ≠ cart math
		return Math.Max(0m, total);
	}
	public string AuthorizePaymentStub(string cardLast4)
	{
		// Pretends to talk to a gateway — auth scheme changes independently of cart rules.
		var payload = $"{GrandTotal():0.00}|{cardLast4}|{_lines.Count}";
		var hash = payload.GetHashCode();
		return $"AUTH-{Math.Abs(hash):X8}";
	}
}


