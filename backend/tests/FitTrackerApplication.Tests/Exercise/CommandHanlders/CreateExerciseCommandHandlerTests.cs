using System.Text;
using FitTracker.Application.Constants;
using FitTracker.Application.Features.Exercise.Commands.CreateExercise;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FitTracker.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FitTrackerApplication.Tests.Exercise.CommandHandlers;

public class CreateExerciseCommandHandlerTests
{
    private readonly Mock<IExerciseReadRepository> _readRepositoryMock;
    private readonly Mock<IExerciseWriteRepository> _writeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock;
    private readonly CreateExerciseCommandHandler _handler;

    public CreateExerciseCommandHandlerTests()
    {
        _readRepositoryMock = new Mock<IExerciseReadRepository>();
        _writeRepositoryMock = new Mock<IExerciseWriteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _blobStorageServiceMock = new Mock<IBlobStorageService>();

        _handler = new CreateExerciseCommandHandler(
            _readRepositoryMock.Object,
            _writeRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _blobStorageServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidCommandAndImage_ShouldUploadImageAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var imageFile = CreateMockFormFile("test-image.jpg", "image/jpeg", 1024);
        var expectedImageUrl = "https://storage.azure.com/images/guid-image.jpg";

        var command = new CreateExerciseCommand(
            "Bench Press",
            MuscleGroup.Chest,
            Equipment.Barbell,
            "A pressing exercise for chest",
            imageFile,
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Bench Press",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _blobStorageServiceMock
            .Setup(x => x.UploadFileAsync(imageFile))
            .ReturnsAsync(expectedImageUrl);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);

        _blobStorageServiceMock.Verify(
            x => x.UploadFileAsync(imageFile),
            Times.Once);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<FitTracker.Domain.Entities.Exercise>(e =>
                    e.Name == "Bench Press" &&
                    e.MuscleGroup == MuscleGroup.Chest &&
                    e.Equipment == Equipment.Barbell &&
                    e.Description == "A pressing exercise for chest" &&
                    e.ImageUrl == expectedImageUrl &&
                    e.CreatedByUserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutImage_ShouldNotCallBlobStorageAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var command = new CreateExerciseCommand(
            "Squat",
            MuscleGroup.Legs,
            Equipment.Barbell,
            null,
            null, // Без изображения
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Squat",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        _blobStorageServiceMock.Verify(
            x => x.UploadFileAsync(It.IsAny<IFormFile>()),
            Times.Never);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<FitTracker.Domain.Entities.Exercise>(e =>
                    e.Name == "Squat" &&
                    e.MuscleGroup == MuscleGroup.Legs &&
                    e.Equipment == Equipment.Barbell &&
                    e.Description == null &&
                    e.ImageUrl == null &&
                    e.CreatedByUserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldTrimWhitespaceFromStrings()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var command = new CreateExerciseCommand(
            "  Deadlift  ",
            MuscleGroup.Back,
            Equipment.Barbell,
            "  A back exercise  ",
            null,
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Deadlift",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<FitTracker.Domain.Entities.Exercise>(e =>
                    e.Name == "Deadlift" &&
                    e.Description == "A back exercise"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldSetCreatedAtAndUpdatedAt()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var command = new CreateExerciseCommand(
            "Test Exercise",
            MuscleGroup.Chest,
            Equipment.Barbell,
            null,
            null,
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Test Exercise",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var beforeExecution = DateTime.UtcNow;

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var afterExecution = DateTime.UtcNow;

        result.IsSuccess.Should().BeTrue();

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<FitTracker.Domain.Entities.Exercise>(e =>
                    e.CreatedAt >= beforeExecution &&
                    e.CreatedAt <= afterExecution &&
                    e.UpdatedAt >= beforeExecution &&
                    e.UpdatedAt <= afterExecution),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_ShouldReturnValidationFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateExerciseCommand(
            "Bench Press",
            MuscleGroup.Chest,
            Equipment.Barbell,
            null,
            null,
            userId);

        var existingExercise = FitTracker.Domain.Entities.Exercise.CreateCustom(
            userId,
            "Bench Press",
            MuscleGroup.Chest,
            Equipment.Barbell);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                command.Name,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExercise);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Errors.Should().ContainSingle();
        result.Error.Errors.First().PropertyName.Should().Be(ErrorKeys.General);
        result.Error.Errors.First().ErrorMessage.Should().Be(DomainErrors.Exercise.AlreadyExists);

        _blobStorageServiceMock.Verify(
            x => x.UploadFileAsync(It.IsAny<IFormFile>()),
            Times.Never);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<FitTracker.Domain.Entities.Exercise>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenBlobStorageFails_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var imageFile = CreateMockFormFile("test.jpg", "image/jpeg", 1024);

        var command = new CreateExerciseCommand(
            "Bench Press",
            MuscleGroup.Chest,
            Equipment.Barbell,
            null,
            imageFile,
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Bench Press",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _blobStorageServiceMock
            .Setup(x => x.UploadFileAsync(imageFile))
            .ThrowsAsync(new Exception("Storage service unavailable"));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Storage service unavailable");

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<FitTracker.Domain.Entities.Exercise>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithLargeImage_ShouldUploadSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var largeImageFile = CreateMockFormFile("large-image.jpg", "image/jpeg", 5 * 1024 * 1024); // 5MB
        var expectedImageUrl = "https://storage.azure.com/images/large-image.jpg";

        var command = new CreateExerciseCommand(
            "Push Up",
            MuscleGroup.Chest,
            Equipment.None,
            null,
            largeImageFile,
            userId);

        _readRepositoryMock
            .Setup(x => x.GetExerciseByName(
                "Push Up",
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((FitTracker.Domain.Entities.Exercise?)null);

        _blobStorageServiceMock
            .Setup(x => x.UploadFileAsync(largeImageFile))
            .ReturnsAsync(expectedImageUrl);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _blobStorageServiceMock.Verify(
            x => x.UploadFileAsync(
                It.Is<IFormFile>(f => f.Length == 5 * 1024 * 1024)),
            Times.Once);

        _writeRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<FitTracker.Domain.Entities.Exercise>(e =>
                    e.ImageUrl == expectedImageUrl),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static IFormFile CreateMockFormFile(string fileName, string contentType, long length)
    {
        var content = new byte[length];
        var stream = new MemoryStream(content);

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.ContentType).Returns(contentType);
        fileMock.Setup(f => f.Length).Returns(length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns((Stream target, CancellationToken token) => stream.CopyToAsync(target, token));

        return fileMock.Object;
    }
}
