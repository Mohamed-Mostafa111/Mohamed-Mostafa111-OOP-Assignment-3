namespace SrpLab;

/// <summary>
/// Grade book: aggregation, letter-band policy, and transcript export.
/// </summary>
public sealed class GradeBook
{

	private readonly Dictionary<string, List<decimal>> _scores;
	private readonly AverageGradeCalculator averageGrade;
	private readonly GradePolicyCalculator gradePolicy;
	private readonly ReportExporter report;

	public GradeBook()
	{
		_scores = new(StringComparer.OrdinalIgnoreCase);
		averageGrade = new AverageGradeCalculator(_scores);
		gradePolicy = new GradePolicyCalculator(averageGrade);
		report = new ReportExporter(gradePolicy, _scores, averageGrade);

	}
	public void Record(string studentId,decimal Score)
	{
	   averageGrade.Record(studentId, Score);
	}
	public string TranscriptPlain(string StudentId,string FullName)
	{
		return report.TranscriptPlain(StudentId, FullName);
	}
}

public sealed class AverageGradeCalculator
{
	private readonly Dictionary<string, List<decimal>> _scores;

	public AverageGradeCalculator(Dictionary<string, List<decimal>> scores)
	{
		_scores = scores;
	}

	public void Record(string studentId, decimal score)
	{
		if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
		if (!_scores.TryGetValue(studentId, out var list))
		{
			list = new List<decimal>();
			_scores[studentId] = list;
		}
		list.Add(score);
	}

	public decimal Average(string studentId)
	{
		if (!_scores.TryGetValue(studentId, out var list) || list.Count == 0) return 0m;
		return Math.Round(list.Average(), 2);
	}
}
public sealed class GradePolicyCalculator
{
	private readonly AverageGradeCalculator averageGrade;

	public GradePolicyCalculator(AverageGradeCalculator averageGrade)
	{
		this.averageGrade = averageGrade;
	}

	public string Letter(string studentId)
	{
		// Academic policy bands change with faculty senate — not with CSV layout.
		var avg =averageGrade.Average(studentId);
		if (avg >= 90) return "A";
		if (avg >= 80) return "B";
		if (avg >= 70) return "C";
		if (avg >= 60) return "D";
		return "F";
	}

	public bool MeetsHonorRoll(string studentId)
	{
		// Extra academic rule set embedded beside averaging.
		return averageGrade.Average(studentId) >= 85 && Letter(studentId) is "A" or "B";
	}
}

public sealed class ReportExporter
{
	private readonly AverageGradeCalculator averageGrade;
	private readonly GradePolicyCalculator gradePolicy;
	private readonly IReadOnlyDictionary<string, List<decimal>> _scores;

	public ReportExporter(GradePolicyCalculator gradePolicy, IReadOnlyDictionary<string, List<decimal>> scores,AverageGradeCalculator averageGrade)
	{
		this.gradePolicy = gradePolicy;
		_scores = scores;
		this.averageGrade = averageGrade;
	}

	public string TranscriptPlain(string studentId, string fullName)
	{
		// Registrar document format ≠ grading policy.
		return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\n" +
			   $"Average: {averageGrade.Average(studentId)}\n" +
			   $"Letter: {gradePolicy.Letter(studentId)}\n" +
			   $"Honor: {gradePolicy.MeetsHonorRoll(studentId)}\n";
	}

	public string ExportCsv()
	{
		var rows = new List<string> { "studentId,average,letter,honor" };
		foreach (var id in _scores.Keys.OrderBy(x => x))
			rows.Add($"{id},{averageGrade.Average(id)},{gradePolicy.Letter(id)},{(gradePolicy.MeetsHonorRoll(id) ? 1 : 0)}");
		return string.Join('\n', rows);
	}
}