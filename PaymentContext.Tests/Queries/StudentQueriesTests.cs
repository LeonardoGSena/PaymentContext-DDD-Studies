using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests;

[TestClass]
public class StudentQueriesTests
{

    private IList<Student> _students;

    public StudentQueriesTests(IList<Student> students)
    {
        for (var i = 0; i <= 10; i++)
        {
            _students.Add(new Student(
                new Name("Aluno", i.ToString()),
                new Document("1111111111" + i.ToString(), Domain.Enums.EDocumentType.CPF),
                new Email(i.ToString() + "@balta.io")
            ));
        }
    }

    [TestMethod]
    public void ShouldReturnNullWhenDocumentDoesNotExist()
    {
        var exp = StudentQueries.GetStudentInfo("12345678911");
        var student = _students.AsQueryable().Where(exp).FirstOrDefault();

        Assert.AreEqual(null, student);
    }

    [TestMethod]
    public void ShouldReturnStudentWhenDocumentExist()
    {
        var exp = StudentQueries.GetStudentInfo("11111111111");
        var student = _students.AsQueryable().Where(exp).FirstOrDefault();

        Assert.AreNotEqual(null, student);
    }
}
