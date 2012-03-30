using System;
using Irrefutable.FinSharp.DateCalculations.DayCounters;
using NUnit.Framework;
using FluentAssertions;

namespace Irrefutable.FinSharp.DateCalculations.Tests
{
    [TestFixture]
    public class DayCounter_30E_360_Fixture
    {
        private DayCounter_30E_360 _dayCounter;

        [SetUp]
        public void SetUp()
        {
            _dayCounter = new DayCounter_30E_360();
        }

        [Test]
        public void NotionalDaysInYear_Returns360_Test()
        {
            Assert.AreEqual(360, _dayCounter.NotionalDaysInYear);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_30October1999_Returns149_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateTime(1999, 06, 1), new DateTime(1999, 10, 30)).Should().Be(149);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_31October1999_Returns150_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateTime(1999, 06, 1), new DateTime(1999, 10, 31)).Should().Be(149);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_1November1999_Returns150_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateTime(1999, 06, 1), new DateTime(1999, 11, 1)).Should().Be(150);
        }
    }
}
