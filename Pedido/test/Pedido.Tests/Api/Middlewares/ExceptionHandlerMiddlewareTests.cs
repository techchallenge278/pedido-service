using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Pedido.Api.Middlewares;
using Pedido.Api.Models;
using Xunit;

namespace Pedido.Tests.Api.Middlewares
{
    public class ExceptionHandlerMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_DeveRetornar400_QuandoInvalidOperationException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlerMiddleware>>();

            RequestDelegate next = _ =>
                throw new InvalidOperationException("Operação inválida");

            var middleware = new ExceptionHandlerMiddleware(next, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            context.Response.Body.Position = 0;
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var response = JsonSerializer.Deserialize<ErrorResponse>(body);

            response.Should().NotBeNull();
            response!.Errors.Should().Contain("Operação inválida");
            response.TraceId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task InvokeAsync_DeveRetornar404_QuandoKeyNotFoundException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlerMiddleware>>();

            RequestDelegate next = _ =>
                throw new KeyNotFoundException("Pedido não encontrado");

            var middleware = new ExceptionHandlerMiddleware(next, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            context.Response.Body.Position = 0;
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var response = JsonSerializer.Deserialize<ErrorResponse>(body);

            response.Should().NotBeNull();
            response!.Errors.Should().Contain("Pedido não encontrado");
        }

        [Fact]
        public async Task InvokeAsync_DeveRetornar500_E_LogarErro_QuandoExceptionNaoTratada()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlerMiddleware>>();

            RequestDelegate next = _ =>
                throw new Exception("Erro inesperado");

            var middleware = new ExceptionHandlerMiddleware(next, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            context.Response.Body.Position = 0;
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var response = JsonSerializer.Deserialize<ErrorResponse>(body);

            response.Should().NotBeNull();
            response!.Errors.Should().Contain("Um erro interno ocorreu. Por favor, tente novamente mais tarde.");

            // 🔥 AQUI ESTÁ A CORREÇÃO DO SEU ERRO 🔥
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
