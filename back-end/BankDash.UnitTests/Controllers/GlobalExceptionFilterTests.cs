using BankDash.Api.Start;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using BankDash.UnitTests.Util;

namespace BankDash.Api.Tests
{
    public class GlobalExceptionFilterTests
    {
        private readonly GlobalExceptionFilter _globalExceptionFilter;
        private readonly Mock<ILogger<GlobalExceptionFilter>> _loggerMock;

        public GlobalExceptionFilterTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
            _globalExceptionFilter = new GlobalExceptionFilter(_loggerMock.Object);
        }

        [Fact]
        public void OnException_ExceptionIsThrown_ReturnServerError()
        {
            // Arrange
            var error = "Test error";
            var context = new ExceptionContext(new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            }, new List<IFilterMetadata>());
            context.Exception = new Exception(error);

            // Act
            _globalExceptionFilter.OnException(context);

            // Assert
            Assert.IsType<JsonResult>(context.Result);
            var jsonResult = context.Result as JsonResult;
            Assert.Equal(500, jsonResult?.StatusCode);
            Assert.Equal(error, JsonValueHelper.GetValue(jsonResult?.Value, "message"));
            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == error),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
    }

        [Fact]
        public void OnException_UnauthorizedAccessExceptionIsThrown_ReturnUnauthorized()
        {
            // Arrange
            var error = "Unauthorized access";
            var context = new ExceptionContext(new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            }, new List<IFilterMetadata>());
            context.Exception = new UnauthorizedAccessException(error);

            // Act
            _globalExceptionFilter.OnException(context);

            // Assert
            Assert.IsType<JsonResult>(context.Result);
            var jsonResult = context.Result as JsonResult;
            Assert.Equal(401, jsonResult.StatusCode);
            Assert.Equal(error, JsonValueHelper.GetValue(jsonResult?.Value, "message"));
            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == error),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
