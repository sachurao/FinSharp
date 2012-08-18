using System;
using StreamCipher.FinSharp.Analytics.Model.Dates;
using StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations;
using NUnit.Framework;
using FluentAssertions;

namespace StreamCipher.FinSharp.Analytics.TimeValue.Tests.DateCalculations
{
    [TestFixture]
    public class DayCounter_Act_360_Fixture
    {
        private DayCounter_Act_360 _dayCounter;

        [SetUp]
        public void SetUp()
        {
            _dayCounter = new DayCounter_Act_360();
        }

        [Test]
        public void NotionalDaysInYear_Returns360_Test()
        {
            _dayCounter.NotionalDaysInYear.Should().Be(360);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_30October1999_Returns151_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateSpan(new DateTime(1999,06,01), new DateTime(1999,10,30))).Should().Be(151);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_31October1999_Returns152_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateSpan(new DateTime(1999, 06, 01), new DateTime(1999, 10, 31))).Should().Be(152);
        }

        [Test]
        public void ComputeDaysBetweenDates_1June1999_1November1999_Returns153_Test()
        {
            _dayCounter.ComputeDaysBetweenDates(new DateSpan(new DateTime(1999, 06, 01), new DateTime(1999, 11,1))).Should().Be(153);
        }
    }
}
