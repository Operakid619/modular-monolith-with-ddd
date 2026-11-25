using System;
using CompanyName.MyMeetings.BuildingBlocks.Application.Queries;
using NUnit.Framework;

namespace CompanyName.MyMeetings.BuildingBlocks.Application.UnitTests.Queries
{
    [TestFixture]
    public class PagedQueryHelperTests
    {
        [TestCase(1, 5, 0, 5)]
        [TestCase(3, 10, 20, 10)]
        [TestCase(null, 20, 0, 20)]
        [TestCase(5, null, 0, int.MaxValue)]
        [TestCase(null, null, 0, int.MaxValue)]
        public void PagedQueryHelper_GetPageData_Test(int? page, int? perPage, int offset, int next)
        {
            IPagedQuery query = new TestQuery(page, perPage);
            var pageData = PagedQueryHelper.GetPageData(query);

            Assert.That(pageData, Is.EqualTo(new PageData(offset, next)));
        }

        [TestCase(0, 10)]
        [TestCase(-1, 10)]
        [TestCase(1, 0)]
        [TestCase(1, -1)]
        public void RejectsNonPositivePagination(int page, int perPage)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                PagedQueryHelper.GetPageData(new TestQuery(page, perPage)));
        }

        [Test]
        public void RejectsOffsetOverflow()
        {
            Assert.Throws<OverflowException>(() =>
                PagedQueryHelper.GetPageData(new TestQuery(int.MaxValue, 2)));
        }

        [Test]
        public void RejectsMissingQuery()
        {
            Assert.Throws<ArgumentNullException>(() => PagedQueryHelper.GetPageData(null));
        }

        private class TestQuery : IPagedQuery
        {
            public TestQuery(int? page, int? perPage)
            {
                Page = page;
                PerPage = perPage;
            }

            public int? Page { get; }

            public int? PerPage { get; }
        }
    }
}