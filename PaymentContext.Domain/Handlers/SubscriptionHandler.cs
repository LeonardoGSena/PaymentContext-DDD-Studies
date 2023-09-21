using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers;

public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>
{
    private readonly IStudentRepository _repository;
    private readonly IEmailService _emailService;

    public SubscriptionHandler(IStudentRepository repository, IEmailService service)
    {
        _repository = repository;
        _emailService = service;
    }

    public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
    {
        // Fail fast validation
        command.Validate();
        if (command.Invalid)
        {
            AddNotifications(command);
            return new CommandResult(false, "Não foi possível realizar sua assinatura");
        }

        // Verificar se o documento ja esta cadastrado
        if (_repository.DocumentExists(command.Document))
            AddNotification("Document", "Este CPF já está em uso");

        // Verificar se o E-mail ja esta cadastrado
        if (_repository.DocumentExists(command.Email))
            AddNotification("E-mail", "Este E-mail já está em uso");

        // Gerar os VOs
        var name = new Name(command.FirstName, command.LastName);
        var document = new Document(command.Document, EDocumentType.CPF);
        var email = new Email(command.Email);
        var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

        // Gerar as Entidades
        var student = new Student(name, document, email);
        var subscription = new Subscription(DateTime.Now.AddMonths(1));
        var payment = new BoletoPayment(
            command.PaidDate,
            command.ExpireDate,
            command.Total,
            command.TotalPaid,
            command.Payer,
            new Document(command.PayerDocument, command.PayerDocumentType),
            address,
            email,
            command.BarCode,
            command.BoletoNumber);

        // Relacionamentos
        subscription.addPayment(payment);
        student.AddSubscription(subscription);

        // Agrupar as validacoes
        if (Invalid)
            return new CommandResult(false, "Não foi possível realizar sua assinatura");

        // Agrupar as Validacoes
        AddNotifications(name, document, email, address, student, subscription, payment);

        // Salvar as Informacoes
        _repository.CreateSubscription(student);

        // Enviar E-mail de boas vindas
        _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

        // Retornar informacoes

        return new CommandResult(true, "Assinatura realizada com sucesso");
    }
}
