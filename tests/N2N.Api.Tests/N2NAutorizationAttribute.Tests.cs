using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using N2N.Api.Filters;
using NUnit.Framework;

namespace N2N.Api.Tests
{
    [TestFixture]
    class N2NAutorizationAttributeTests
    {
        [Test]
        public void Should_pass_all_if_action_has_IAllowAnonymous_attribute()
        {
            var filter = new N2NAutorizationAttribute();
            var contextMock = new Mock<ActionExecutingContext>();
            contextMock.Setup(x => x.Filters).Returns(new List<IFilterMetadata>() {new AllowAnonymousFilter()});



            filter.OnActionExecuting(contextMock.Object);
        }
    }
}
