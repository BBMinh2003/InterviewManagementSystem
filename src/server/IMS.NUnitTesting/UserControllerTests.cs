using AutoMapper;
using IMS.API.Controllers;
using IMS.Business.Handlers.UserHandlers;
using IMS.Business.ViewModels.UserViews;
using IMS.Data;
using IMS.Models.Common;
using IMS.Models.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class UserController_UnitTests
{
    private Mock<IMediator> _mediatorMock;
    private UserController _controller;
    private IMSDbContext _dbContext;
    private Role _testRole;
    private Department _testDepartment;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<IMSDbContext>()
            .UseInMemoryDatabase($"IMS_TestDb_Controller_{Guid.NewGuid()}")
            .Options;
        _dbContext = new IMSDbContext(options);

        _testRole = new Role { Id = Guid.NewGuid(), Name = "Tester", NormalizedName = "TESTER" };
        _testDepartment = new Department { Id = Guid.NewGuid(), Name = "Testing Dept" };
        _dbContext.Roles.Add(_testRole);
        _dbContext.Departments.Add(_testDepartment);
        _dbContext.SaveChanges();

        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task CreateUser_ValidCommand_ReturnsCreatedAtActionResultWithViewModel()
    {
        var command = new UserCreateCommand
        {
            FullName = "John Doe",
            Email = "john.doe@example.com",
            Address = "123 Main St",
            PhoneNumber = "1234567890",
            Gender = Gender.MALE,
            RoleId = _testRole.Id,
            DepartmentId = _testDepartment.Id,
            DateOfBirth = new DateTime(2003, 12, 04),
            IsActive = true,
            Note = "This is a test user."
        };

        var expectedResultViewModel = new UserViewModel
        {
            Id = Guid.NewGuid(),
            FullName = command.FullName,
            Email = command.Email
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UserCreateCommand>(), CancellationToken.None))
                     .ReturnsAsync(expectedResultViewModel);

        var actionResult = await _controller.Create(command);

        Assert.That(actionResult, Is.InstanceOf<CreatedAtActionResult>());
        var createdAtResult = actionResult as CreatedAtActionResult;
        var actualViewModel = createdAtResult?.Value as UserViewModel;
        Assert.That(createdAtResult.StatusCode, Is.EqualTo(201));
        Assert.That(actualViewModel, Is.Not.Null);
        Assert.That(actualViewModel.FullName, Is.EqualTo(expectedResultViewModel.FullName));

        Assert.That(createdAtResult.ActionName, Is.EqualTo(nameof(UserController.Create)));
        Assert.That(createdAtResult.Value, Is.InstanceOf<UserViewModel>());
        Assert.That(createdAtResult.Value, Is.EqualTo(expectedResultViewModel));
        _mediatorMock.Verify(m => m.Send(command, CancellationToken.None), Times.Once);
        Assert.That(_dbContext.Users.FirstOrDefault(u => u.Email == command.Email), Is.Null);
    }

    // [Test]
    public async Task CreateUser_HandlerThrowsException_ReturnsBadRequest()
    {
        var command = new UserCreateCommand
        {
            FullName = "John Doe",
            Email = "john.doe@example.com",
            Address = "123 Main St",
            PhoneNumber = "1234567890",
            Gender = Gender.MALE,
            RoleId = _testRole.Id,
            DepartmentId = _testDepartment.Id,
            DateOfBirth = new DateTime(2003, 12, 04),
            IsActive = true,
            Note = "This is a test user."
        };
        const string exceptionMessage = "Handler failed validation";

        _mediatorMock.Setup(m => m.Send(It.IsAny<UserCreateCommand>(), CancellationToken.None))
                     .ThrowsAsync(new InvalidOperationException(exceptionMessage));

        var actionResult = await _controller.Create(command);

        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = actionResult as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(exceptionMessage));
        _mediatorMock.Verify(m => m.Send(command, CancellationToken.None), Times.Once);
    }
}