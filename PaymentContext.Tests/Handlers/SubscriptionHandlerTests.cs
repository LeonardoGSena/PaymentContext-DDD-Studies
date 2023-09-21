using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handlers;

[TestClass]
public class SubscriptionHandlerTests
{
    [TestMethod]
    public void ShouldReturnErrorWhenDocumentsExists()
    {
        var handler = new SubscriptionHandler(
            new FakeStudentRepository(),
            new FakeEmailService()
        );

        var command = new CreateBoletoSubscriptionCommand();
        command.FirstName = "Bruce";
        command.LastName = "Wayne";
        command.Document = "99999999999";
        command.Email = "hello@balta.io2";
        command.BarCode = "123456789";
        command.BoletoNumber = "1234567";
        command.PaymentNumber = "123121";
        command.PaidDate = DateTime.Now;
        command.ExpireDate = DateTime.Now.AddMonths(1);
        command.Total = 60;
        command.TotalPaid = 60;
        command.Payer = "WAYNE CORP";
        command.PayerDocument = "123456678911";
        command.PayerDocumentType = Domain.Enums.EDocumentType.CPF;
        command.PayerEmail = "batman@dc.com";
        command.Street = "asdas";
        command.Number = "asdd";
        command.Neighborhood = "lksajdf";
        command.City = "as";
        command.State = "as";
        command.Country = "as";
        command.ZipCode = "12346578";

        handler.Handle(command);

        Assert.AreEqual(false, handler.Valid);
    }
}
