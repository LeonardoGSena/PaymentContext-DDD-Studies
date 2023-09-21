using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests;

[TestClass]
public class StudentTests
{
    private readonly Name _name;
    private readonly Document _document;
    private readonly Address _address;
    private readonly Email _email;
    private readonly Student _student;
    private readonly Subscription _subscription;

    public StudentTests()
    {
        _name = new Name("Leonardo", "Sena");
        _document = new Document("34225545806", EDocumentType.CPF);
        _email = new Email("leonardo@balta.io");
        _address = new Address("Rua aaa", "1234", "neighbo", "city of God", "PE", "BR", "54111222");
        _student = new Student(_name, _document, _email);
        _subscription = new Subscription(null);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenActiveSubscription()
    {

        var payment = new PayPalPayment(DateTime.Now, DateTime.Now.AddDays(5), 800, 800, "Leo Corp", _document, _address, _email, "12346");

        _subscription.addPayment(payment);
        _student.AddSubscription(_subscription);
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Invalid);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
    {
        var payment = new PayPalPayment(DateTime.Now, DateTime.Now.AddDays(5), 800, 0, "Leo Corp", _document, _address, _email, "12346");

        _subscription.addPayment(payment);
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Invalid);
    }

    [TestMethod]
    public void ShouldReturnSuccessWhenAddSubscription()
    {
        var payment = new PayPalPayment(DateTime.Now, DateTime.Now.AddDays(5), 800, 800, "Leo Corp", _document, _address, _email, "12346");

        _subscription.addPayment(payment);
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Valid);
    }
}
