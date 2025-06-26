using System;
using System.Collections.Generic;
using System.Linq;
using task02;
using Xunit;

public class StudentServiceTests
{
    private readonly List<Student> _testStudents;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Мария", Faculty = "Экономика", Grades = new List<int> { 4, 4, 3 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        // Arrange
        var faculty = "ФИТ";
        
        // Act
        var result = _service.GetStudentsByFaculty(faculty).ToList();
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == faculty));
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        // Arrange
        var minAverageGrade = 4.0;
        
        // Act
        var result = _service.GetStudentsWithMinAverageGrade(minAverageGrade).ToList();
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Name == "Иван");
        Assert.Contains(result, s => s.Name == "Петр");
        Assert.True(result.All(s => s.Grades.Average() >= minAverageGrade));
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsStudentsInCorrectOrder()
    {
        // Act
        var result = _service.GetStudentsOrderedByName().ToList();
        
        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal("Анна", result[0].Name);
        Assert.Equal("Иван", result[1].Name);
        Assert.Equal("Мария", result[2].Name);
        Assert.Equal("Петр", result[3].Name);
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        // Act
        var result = _service.GroupStudentsByFaculty();
        
        // Assert
        Assert.Equal(2, result.Count);
        
        var fitStudents = result["ФИТ"].ToList();
        Assert.Equal(2, fitStudents.Count);
        Assert.Contains(fitStudents, s => s.Name == "Иван");
        Assert.Contains(fitStudents, s => s.Name == "Анна");
        
        var economyStudents = result["Экономика"].ToList();
        Assert.Equal(2, economyStudents.Count);
        Assert.Contains(economyStudents, s => s.Name == "Петр");
        Assert.Contains(economyStudents, s => s.Name == "Мария");
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        // Act
        var result = _service.GetFacultyWithHighestAverageGrade();
        
        // Assert
        Assert.Equal("Экономика", result);
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_WithEmptyList_ReturnsNull()
    {
        // Arrange
        var emptyService = new StudentService(new List<Student>());
        
        // Act
        var result = emptyService.GetFacultyWithHighestAverageGrade();
        
        // Assert
        Assert.Null(result);
    }
}
