using System;
using Loans.Domain.Applications;
using NUnit.Framework;
using Moq;

namespace Loans.Tests
{
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                                                  product,
                                                  amount,
                                                  "Sarah",
                                                  25,
                                                  "133 Pluralsight Drive, Draper, Utah",
                                                  64_999);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,
                                                   mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.False);
        }


        delegate void ValidateCallback(string applicantName, 
                                       int applicantAge, 
                                       string applicantAddress, 
                                       ref IdentityVerificationStatus status);


        [Test]
        public void Accept()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                                                  product,
                                                  amount,
                                                  "Sarah",
                                                  25,
                                                  "133 Pluralsight Drive, Draper, Utah",
                                                  65_000);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();

            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                                                       25,
                                                       "133 Pluralsight Drive, Draper, Utah"))
                                .Returns(true);



            var mockCreditScorer = new Mock<ICreditScorer>();

            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
            mockCreditScorer.SetupProperty(x => x.Count);


            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,
                                                   mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.True);
            Assert.That(mockCreditScorer.Object.Count, Is.EqualTo(1));
        }

        [Test]
        public void NullReturnExample()
        {
            var mock = new Mock<INullExample>();

            mock.Setup(x => x.SomeMethod());
                //.Returns<string>(null);

            string mockReturnValue = mock.Object.SomeMethod();

            Assert.That(mockReturnValue, Is.Null);
        }
    }

    public interface INullExample
    {
        string SomeMethod();
    }
}
