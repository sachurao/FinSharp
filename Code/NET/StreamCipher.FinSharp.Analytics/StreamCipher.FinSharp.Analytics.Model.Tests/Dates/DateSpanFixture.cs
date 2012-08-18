using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StreamCipher.FinSharp.Analytics.Model.Dates;

namespace StreamCipher.FinSharp.Analytics.Model.Tests.Dates
{
    [TestFixture]
    public class DateSpanFixture
    {
        private DateTime _today;
        private DateTime _yest;

        [SetUp]
        public void SetUp()
        {
            _today = DateTime.Today;
            _yest = _today.AddDays(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void New_WhenEndDateBeforeStartDate_ThrowsException()
        {
            var ds = new DateSpan(_today, _yest);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void New_WhenEndDateOnStartDate_ThrowsException()
        {
            var ds = new DateSpan(_today, _today);
        }
    }
}
