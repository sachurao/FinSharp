using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Irrefutable.FinSharp.DateCalculations.Tests
{
    [TestFixture]
    public class DateSequenceFactory_Fixture
    {
        private DateSequenceFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new DateSequenceFactory();
        }

        [Test]
        public void CreateDateSequence_SemiAnnualBetween15Oct1999And15Oct2000_Returns15April2000_Test()
        {
            IList<DateTime> retVal = _factory.CreateDateSequence(
                new DateSpan(new DateTime(1999, 10, 15), new DateTime(2000, 10, 15)),
                DateSequenceFrequency.SEMIANNUAL).ToList();

            retVal.Count().Should().Be(1);
            retVal[0].Should().Be(new DateTime(2000, 04, 15));
        }

        [TestCase(DateSequenceFrequency.ANNUAL, 1999,10,15,2002,10,15, Result=2 )]
        [TestCase(DateSequenceFrequency.SEMIANNUAL, 1999, 10, 15, 2002, 10, 15, Result = 5)]

        public int CreateDateSequence_ForwardGeneration_ParameterizedTest(DateSequenceFrequency frequency,
            int y1, int m1, int d1, int y2, int m2, int d2)
        {
            IList<DateTime> retVal = _factory.CreateDateSequence(
                new DateSpan(new DateTime(y1, m1, d1), new DateTime(y2, m2, d2)),
                frequency).ToList();

            return retVal.Count();
        }

    }
}
