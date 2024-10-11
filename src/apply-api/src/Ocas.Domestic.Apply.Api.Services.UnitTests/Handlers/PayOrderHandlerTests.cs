using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Clients;
using Ocas.Domestic.Apply.Services.Handlers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data.Extras;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnums = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class PayOrderHandlerTests
    {
        private readonly AllLookups _lookups;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILogger<PayOrderHandler> _logger;
        private readonly RequestCache _requestCache;

        public PayOrderHandlerTests()
        {
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookups = _models.AllApplyLookups;
            _user = Mock.Of<IPrincipal>();
            _logger = Mock.Of<ILogger<PayOrderHandler>>();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldThrow_WhenApplicantNotComplete()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(new Dto.Order
            {
                ApplicantId = Guid.NewGuid(),
                FinalTotal = 0
            });
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.PersonalInformation);

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, Mock.Of<IMonerisClient>(), new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().WithMessage("Applicant profile is not complete")
                .And.ErrorCode.Should().Be(ErrorCodes.Applicant.IncompleteError);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldThrow_WhenApplicationIncomplete()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 1,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = _dataFakerFixture.Faker.Random.Bool() ? (int?)null : 0
            });
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            var monerisServiceMock = new Mock<IMonerisClient>();

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().WithMessage($"Application {applicationId} is incomplete");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldLockPayment_WhenCrmFails()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 1,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = (int)DtoEnums.ApplicationCompletedSteps.TranscriptRequests
            }));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.CreatePaymentTransactionDetail(It.IsAny<Dto.ReceiptBase>())).Throws(new Exception());
            var monerisServiceMock = new Mock<IMonerisClient>();
            monerisServiceMock.Setup(m => m.ChargeCard(
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(new ChargeCardResult
                {
                    ChargeSuccess = true,
                    ReceiptBase = new Dto.ReceiptBase()
                }));

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            contact.PaymentLocked.Should().BeFalse();
            func.Should().Throw<Exception>();
            contact.PaymentLocked.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldCompletePayment_WhenTrUpdateFails()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var transcriptRequestId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 1,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    },
                    new Dto.OrderDetail
                    {
                        Amount = 8M,
                        Type = DtoEnums.ShoppingCartItemType.TranscriptRequestFee,
                        ReferenceId = transcriptRequestId
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicationId = applicationId,
                TranscriptRequestStatusId = _lookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id
            };
            var transcriptRequests = new List<Dto.TranscriptRequest> { transcriptRequest } as IList<Dto.TranscriptRequest>;
            var financialTransaction = new Dto.FinancialTransaction();
            var financialTransactions = new List<Dto.FinancialTransaction> { financialTransaction } as IList<Dto.FinancialTransaction>;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = (int)DtoEnums.ApplicationCompletedSteps.TranscriptRequests
            }));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(transcriptRequests);
            domesticContextMock.Setup(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>())).Throws(new Exception());
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>())).ReturnsAsync(financialTransactions);
            var monerisServiceMock = new Mock<IMonerisClient>();
            monerisServiceMock.Setup(m => m.ChargeCard(
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new ChargeCardResult
                {
                    ChargeSuccess = true,
                    ReceiptBase = new Dto.ReceiptBase()
                }));

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            contact.PaymentLocked.Should().BeFalse();
            domesticContextMock.Verify(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>()), Times.Once);
            domesticContextMock.Verify(m => m.CreatePaymentTransactionDetail(It.IsAny<Dto.ReceiptBase>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldRollbackTrs_WhenPaymentFails()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.HostedTokenization);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var transcriptRequestId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 103,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    },
                    new Dto.OrderDetail
                    {
                        Amount = 8M,
                        Type = DtoEnums.ShoppingCartItemType.TranscriptRequestFee,
                        ReferenceId = transcriptRequestId
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicationId = applicationId,
                TranscriptRequestStatusId = _lookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id
            };
            var transcriptRequests = new List<Dto.TranscriptRequest> { transcriptRequest } as IList<Dto.TranscriptRequest>;
            var financialTransaction = new Dto.FinancialTransaction();
            var financialTransactions = new List<Dto.FinancialTransaction> { financialTransaction } as IList<Dto.FinancialTransaction>;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = (int)DtoEnums.ApplicationCompletedSteps.TranscriptRequests
            }));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.CreatePaymentTransactionDetail(It.IsAny<Dto.ReceiptBase>())).Throws(new Exception());
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(transcriptRequests);
            domesticContextMock.Setup(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>())).Throws(new Exception());
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>())).ReturnsAsync(financialTransactions);
            var monerisServiceMock = new Mock<IMonerisClient>();
            monerisServiceMock.Setup(m => m.ChargeCard(
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new ChargeCardResult
                {
                    ChargeSuccess = true,
                    ReceiptBase = new Dto.ReceiptBase()
                }));

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            contact.PaymentLocked.Should().BeFalse();
            func.Should().Throw<Exception>();
            contact.PaymentLocked.Should().BeTrue();
            domesticContextMock.Verify(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>()), Times.Exactly(2));
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldCompleteZeroDollarPayment_WhenTrUpdateFails()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.ZeroDollar);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var transcriptRequestId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 0,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    },
                    new Dto.OrderDetail
                    {
                        Amount = 8M,
                        Type = DtoEnums.ShoppingCartItemType.TranscriptRequestFee,
                        ReferenceId = transcriptRequestId
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicationId = applicationId,
                TranscriptRequestStatusId = _lookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id
            };
            var transcriptRequests = new List<Dto.TranscriptRequest> { transcriptRequest } as IList<Dto.TranscriptRequest>;
            var financialTransaction = new Dto.FinancialTransaction();
            var financialTransactions = new List<Dto.FinancialTransaction> { financialTransaction } as IList<Dto.FinancialTransaction>;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = (int)DtoEnums.ApplicationCompletedSteps.TranscriptRequests
            }));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(transcriptRequests);
            domesticContextMock.Setup(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>())).Throws(new Exception());
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>())).ReturnsAsync(financialTransactions);
            var monerisServiceMock = new Mock<IMonerisClient>();
            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.WriteDetailsToOrder(
                    It.IsAny<Dto.Contact>(),
                    It.IsAny<Dto.Application>(),
                    It.IsAny<Dto.Order>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<Dto.PaymentMethod>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>()))
                .ReturnsAsync(order);

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, domesticContextExtrasMock.Object, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            contact.PaymentLocked.Should().BeFalse();
            domesticContextMock.Verify(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>()), Times.Once);
            domesticContextExtrasMock.Verify(
                m => m.WriteDetailsToOrder(
                It.IsAny<Dto.Contact>(),
                It.IsAny<Dto.Application>(),
                It.IsAny<Dto.Order>(),
                It.IsAny<string>(),
                It.IsAny<IList<Dto.PaymentMethod>>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string>(),
                It.IsAny<decimal>()),
                Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void PayOrder_ShouldRollbackTrs_WhenZeroDollarPaymentFails()
        {
            // Arrange
            var payOrderInfo = _models.GetPayOrderInfo().Generate(Constants.Payment.RuleSet.ZeroDollar);
            var request = new PayOrder
            {
                User = _user,
                PayOrderInfo = payOrderInfo
            };
            var applicantId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var transcriptRequestId = Guid.NewGuid();
            var order = new Dto.Order
            {
                ApplicantId = applicantId,
                ApplicationId = applicationId,
                ApplicationCycleStatusId = _lookups.ApplicationCycleStatuses
                    .Single(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id,
                FinalTotal = 0,
                OrderPaymentStatus = DtoEnums.OrderPaymentStatus.Pending,
                Details = new List<Dto.OrderDetail>
                {
                    new Dto.OrderDetail
                    {
                        Amount = 95M,
                        Type = DtoEnums.ShoppingCartItemType.ApplicationFee
                    },
                    new Dto.OrderDetail
                    {
                        Amount = 8M,
                        Type = DtoEnums.ShoppingCartItemType.TranscriptRequestFee,
                        ReferenceId = transcriptRequestId
                    }
                }
            };
            var contact = new Dto.Contact
            {
                Id = applicantId,
                PaymentLocked = false
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicationId = applicationId,
                TranscriptRequestStatusId = _lookups.TranscriptRequestStatuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id
            };
            var transcriptRequests = new List<Dto.TranscriptRequest> { transcriptRequest } as IList<Dto.TranscriptRequest>;
            var financialTransaction = new Dto.FinancialTransaction();
            var financialTransactions = new List<Dto.FinancialTransaction> { financialTransaction } as IList<Dto.FinancialTransaction>;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOrder(It.IsAny<Guid>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(DtoEnums.CompletedSteps.Experience);
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(order);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Application
            {
                ApplicantId = applicantId,
                Id = applicationId,
                CompletedSteps = (int)DtoEnums.ApplicationCompletedSteps.TranscriptRequests
            }));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.UpdateContact(It.IsAny<Dto.Contact>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(transcriptRequests);
            domesticContextMock.Setup(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>())).Throws(new Exception());
            domesticContextMock.Setup(m => m.GetFinancialTransactions(It.IsAny<Dto.GetFinancialTransactionOptions>())).ReturnsAsync(financialTransactions);
            var monerisServiceMock = new Mock<IMonerisClient>();
            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.WriteDetailsToOrder(
                It.IsAny<Dto.Contact>(),
                It.IsAny<Dto.Application>(),
                It.IsAny<Dto.Order>(),
                It.IsAny<string>(),
                It.IsAny<IList<Dto.PaymentMethod>>(),
                It.IsAny<bool>(),
                It.IsAny<Guid?>(),
                It.IsAny<string>(),
                It.IsAny<decimal>()))
                .Throws(new Exception());

            var handler = new PayOrderHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, monerisServiceMock.Object, domesticContextExtrasMock.Object, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<Exception>();
            contact.PaymentLocked.Should().BeFalse();
            domesticContextMock.Verify(m => m.UpdateTranscriptRequest(It.IsAny<Dto.TranscriptRequest>()), Times.Exactly(2));
        }
    }
}
