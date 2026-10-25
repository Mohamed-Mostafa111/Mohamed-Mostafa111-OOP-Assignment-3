namespace RefactoringLab.Part03.Students;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public static class StudentCatalog
{
	// refactoring to use yield return to avoid loading all students into memory at once.
	public static IEnumerable<Student> GetAllStudents()
    {
        for (var i = 1; i <= 1_000_000; i++)
        {
            yield return new Student
			{
				Id = i,
				Name = $"Student {i}"
			};

        }
    }
}
