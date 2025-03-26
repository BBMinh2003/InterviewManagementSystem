using AutoMapper;
using IMS.API.ConfigurationOptions;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using IMS.Models.Security;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace IMS.NUnitTesting;

[TestFixture]
public class CandidateHandlerTests
{
    private IMSDbContext _dbContext;

    private IMapper _mapper;

    private IUnitOfWork _unitOfWork;

    private User _testUser;

    private List<Candidate> _candidates;

    private string _databaseName;

    private Mock<IUserIdentity> _userIdentityMock;

    [SetUp]
    public void Setup()
    {
        _databaseName = $"IMS_Test_{Guid.NewGuid()}";

        var options = new DbContextOptionsBuilder<IMSDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;

        _dbContext = new IMSDbContext(options);

        // Setup AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        // Setup test data
        _testUser = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Test User",
            Email = "test@example.com",
            UserName = "testuser",
            IsActive = true
        };

        _dbContext.Users.Add(_testUser);
        _dbContext.SaveChanges();

        var position = new Position { Id = Guid.Parse("e1c2d3f4-5678-90ab-cdef-1234567890ab"), Name = "Backend Developer" };
        var skills = new List<Skill>
        {
            new Skill { Id = Guid.Parse("f99e1df1-8b1d-4b3d-82b9-3a0b16e4208f"), Name = "C#" },
            new Skill { Id = Guid.Parse("13f40a78-d861-463a-8ad1-bf12cc449f2a"), Name = "SQL" }
        };

        _candidates = new List<Candidate>
        {
            new Candidate
            {
                Id = Guid.Parse("d2f6e5a8-1c4d-4d9a-b73b-94d0c81a21ec"),
                Name = "TestCandidate1",
                Email = "test1@example.com",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Address = "Ha Noi",
                Phone = "0123456789",
                Gender = Gender.MALE,
                CV_Attachment = "cv1.pdf",
                RecruiterOwner = _testUser,
                HighestLevel = HighestLevel.BachelorsDegree,
                CandidateSkills = new List<CandidateSkill>
                {
                    new CandidateSkill { Skill = skills[0] },
                    new CandidateSkill { Skill = skills[1] }
                }
            },
            new Candidate
            {
                Id = Guid.Parse("a1e2f3c4-d5b6-4e7f-8910-123456789abc"),
                Name = "TestCandidate2",
                Email = "test2@example.com",
                DateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Address = "Ho Chi Minh",
                Phone = "0987654321",
                Gender = Gender.MALE,
                CV_Attachment = "cv2.pdf",
                RecruiterOwner = _testUser,
                HighestLevel = HighestLevel.MastersDegree,
                Position = position
            }
        };

        _dbContext.AddRange(position);
        _dbContext.AddRange(skills);
        _dbContext.AddRange(_candidates);
        _dbContext.SaveChanges();

        // Setup mock user identity
        _userIdentityMock = new Mock<IUserIdentity>();
        _userIdentityMock.Setup(ui => ui.UserId).Returns(_testUser.Id);
        _userIdentityMock.Setup(ui => ui.UserName).Returns(_testUser.UserName);

        // Setup UnitOfWork with the real DbContext and mock user identity
        _unitOfWork = new UnitOfWork(_dbContext, _userIdentityMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _unitOfWork.Dispose();
        _dbContext.Dispose();
    }

    [Test]
    public async Task CandidateCreate_Handle_ValidRequest_ReturnsCandidateViewModel()
    {
        // Arrange
        var position = new Position { Id = Guid.Parse("99999999-cccc-cccc-cccc-cccccccccccc"), Name = "Tester" };
        var skill = new Skill { Id = Guid.Parse("ffffffff-cccc-cccc-cccc-cccccccccccc"), Name = "Testing" };

        _dbContext.AddRange(position, skill);
        await _dbContext.SaveChangesAsync();

        var handler = new CandidateCreateCommandHandler(_unitOfWork, _mapper);
        var request = new CandidateCreateCommand
        {
            Name = "Tran Quang Manh",
            Email = "manh@gmail.com",
            DateOfBirth = new DateTime(2003, 3, 26, 0, 0, 0, DateTimeKind.Utc),
            Address = "Ha Noi",
            Phone = "0987654323",
            Gender = Gender.MALE,
            CV_Attachment = "manh_cv.pdf",
            Note = "string",
            Status = CandidateStatus.WaitingForApproval,
            YearOfExperience = 0,
            HighestLevel = HighestLevel.BachelorsDegree,
            PositionId = position.Id,
            RecruiterOwnerId = _testUser.Id,
            CandidateSkillIds = [skill.Id]
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(request.Name));
            Assert.That(_dbContext.Candidates.Count(), Is.EqualTo(3));
        });
    }

    [Test]
    public void CandidateCreate_Handle_DuplicateEmail_ThrowsResourceUniqueException()
    {
        // Arrange
        var existingCandidate = new Candidate
        {
            Name = "John Doe",
            Email = "existing@example.com",
            DateOfBirth = DateTime.UtcNow,
            Address = "123 Main St",
            Phone = "0912345678",
            Gender = Gender.MALE,
            CV_Attachment = "cv.pdf",
            HighestLevel = HighestLevel.BachelorsDegree,
        };
        _dbContext.Candidates.Add(existingCandidate);
        _dbContext.SaveChanges();

        var handler = new CandidateCreateCommandHandler(_unitOfWork, _mapper);
        var request = new CandidateCreateCommand
        {
            Name = "John Doe",
            Email = "existing@example.com",
            DateOfBirth = DateTime.UtcNow,
            Address = "123 Main St",
            Phone = "0912345678",
            Gender = Gender.MALE,
            CV_Attachment = "cv.pdf",
            HighestLevel = HighestLevel.BachelorsDegree,
        };

        // Act & Assert
        Assert.ThrowsAsync<ResourceUniqueException>(() =>
            handler.Handle(request, CancellationToken.None));
    }

    // DeletedById is fixed
    [Test]
    public async Task CandidateDeleteById_Handle_ValidId_SoftDeletesCandidateAndReturnsTrue()
    {
        // Arrange
        var handler = new CandidateDeleteByIdCommandHandler(_unitOfWork, _mapper);
        var command = new CandidateDeleteByIdCommand { Id = _candidates[0].Id };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedCandidate = await _dbContext.Candidates
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == _candidates[0].Id);
        if (deletedCandidate != null)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(deletedCandidate.IsDeleted, Is.True);
                Assert.That(deletedCandidate.DeletedById, Is.EqualTo(Guid.Parse("00000000-0000-0000-0000-000000000000")));
                Assert.That(deletedCandidate.DeletedAt, Is.Not.Null);
            });
        }
        else
        {
            Assert.Fail("Candidate with specified Id not found.");
        }
    }

    [Test]
    public void CandidateDeleteById_Handle_InvalidId_ThrowsResourceNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var handler = new CandidateDeleteByIdCommandHandler(_unitOfWork, _mapper);
        var command = new CandidateDeleteByIdCommand { Id = invalidId };

        // Act & Assert
        Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task CandidateGetAllQuery_Handle_ReturnsAllCandidatesWithRelations()
    {
        // Arrange
        // Đảm bảo dữ liệu test được tạo đúng
        var candidatesInDb = await _dbContext.Candidates.ToListAsync();
        Assert.That(candidatesInDb, Has.Count.EqualTo(2)); // Kiểm tra dữ liệu đã được seed

        var handler = new CandidateGetAllQueryHandler(_unitOfWork, _mapper);
        var query = new CandidateGetAllQuery();

        // Act
        var result = (await handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].PositionName, Is.EqualTo("Backend Developer"));
            Assert.That(result[0].RecruiterOwnerName, Is.EqualTo("Test User")); // Kiểm tra tên recruiter đúng
            Assert.That(result[0].CandidateSkills.Select(s => s.Name), Is.EquivalentTo(new[] { "C#", "SQL" }));
            Assert.That(result[1].CandidateSkills, Is.Empty);
        });
    }

    [Test]
    public async Task CandidateGetAllQuery_Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        await _dbContext.Database.EnsureDeletedAsync();
        var handler = new CandidateGetAllQueryHandler(_unitOfWork, _mapper);

        // Act
        var result = await handler.Handle(new CandidateGetAllQuery(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task CandidateGetByIdQuery_Handle_ValidId_ReturnsCandidateViewModel()
    {
        // Arrange
        var handler = new CandidateGetByIdQueryHandler(_unitOfWork, _mapper);
        var query = new CandidateGetByIdQuery { Id = _candidates[0].Id };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(_candidates[0].Id));
            Assert.That(result.Name, Is.EqualTo(_candidates[0].Name));
            Assert.That(result.Email, Is.EqualTo(_candidates[0].Email));
        });
    }

    [Test]
    public void CandidateGetByIdQuery_Handle_InvalidId_ThrowsResourceNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var handler = new CandidateGetByIdQueryHandler(_unitOfWork, _mapper);
        var query = new CandidateGetByIdQuery { Id = invalidId };

        // Act & Assert
        Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
    }

    [Test]
    public async Task Handle_UpdateBasicInfo_ReturnsUpdatedCandidate()
    {
        // Arrange
        var handler = new CandidateUpdateCommandHandler(_unitOfWork, _mapper);
        var request = new CandidateUpdateCommand
        {
            Id = _candidates[0].Id,
            Name = "Updated Name",
            Email = "updated@example.com",
            YearOfExperience = 5,
            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Address = "Ha Noi",
            Phone = "0123456789",
            Gender = Gender.MALE,
            CV_Attachment = "cv1.pdf",
            HighestLevel = HighestLevel.BachelorsDegree,
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedCandidate = await _dbContext.Candidates.FindAsync(_candidates[0].Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo("Updated Name"));

            if (updatedCandidate != null)
            {
                Assert.That(updatedCandidate.Email, Is.EqualTo("updated@example.com"));
                Assert.That(updatedCandidate.YearOfExperience, Is.EqualTo(5));
            }
            else
            {
                Assert.Fail("Candidate with specified Id not found.");
            }
        });
    }

    [Test]
    public async Task Handle_ValidCandidate_ShouldBanAndReturnTrue()
    {
        // Arrange
        var candidate = _candidates[0];
        candidate.Status = CandidateStatus.WaitingForApproval; 
        await _dbContext.SaveChangesAsync();

        var handler = new CandidateBanByIdCommandHandler(_unitOfWork, _mapper);
        var command = new CandidateBanByIdCommand { Id = candidate.Id };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCandidate = await _dbContext.Candidates.FindAsync(candidate.Id);
        if (updatedCandidate != null)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(updatedCandidate.Status, Is.EqualTo(CandidateStatus.Banned));
            });
        }
        else
        {
            Assert.Fail("Candidate with specified Id not found.");
        }
    }

    [Test]
    public async Task Handle_AlreadyBannedCandidate_ShouldReturnFalse()
    {
        // Arrange
        var candidate = _candidates[0];
        candidate.Status = CandidateStatus.Banned; 
        await _dbContext.SaveChangesAsync();

        var handler = new CandidateBanByIdCommandHandler(_unitOfWork, _mapper);
        var command = new CandidateBanByIdCommand { Id = candidate.Id };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.False);
    }
}
