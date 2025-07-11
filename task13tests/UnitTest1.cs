using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using task13;

namespace task13tests
{
    public class StudentJsonSerializerTests
    {
        [Fact]
        public void Serialize_ValidStudent_ReturnsCorrectJson()
        {
            var student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(2000, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 5 },
                    new Subject { Name = "Physics", Grade = 4 }
                }
            };

            var json = StudentJsonSerializer.Serialize(student);
            
            Assert.Contains("\"firstName\": \"John\"", json);
            Assert.Contains("\"lastName\": \"Doe\"", json);
            Assert.Contains("\"birthDate\": \"2000-01-01\"", json);
            Assert.Contains("\"grades\"", json);
        }

        [Fact]
        public void Deserialize_ValidJson_ReturnsStudentObject()
        {
            string json = @"{
                ""firstName"": ""John"",
                ""lastName"": ""Doe"",
                ""birthDate"": ""2000-01-01"",
                ""grades"": [
                    { ""name"": ""Math"", ""grade"": 5 },
                    { ""name"": ""Physics"", ""grade"": 4 }
                ]
            }";
            var student = StudentJsonSerializer.Deserialize(json);
            Assert.Equal("John", student.FirstName);
            Assert.Equal("Doe", student.LastName);
            Assert.Equal(new DateTime(2000, 1, 1), student.BirthDate);
            Assert.Equal(2, student.Grades.Count);
        }
        [Fact]
        public void SaveAndLoad_ValidStudent_WorksCorrectly()
        {
            var student = new Student
            {
                FirstName = "Alice",
                LastName = "Smith",
                BirthDate = new DateTime(1999, 5, 15),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Chemistry", Grade = 4 },
                    new Subject { Name = "Biology", Grade = 5 }
                }
            };
            string filePath = "test_student.json";

            try
            {
                StudentJsonSerializer.SaveToFile(student, filePath);
                var loadedStudent = StudentJsonSerializer.LoadFromFile(filePath);
                Assert.Equal(student.FirstName, loadedStudent.FirstName);
                Assert.Equal(student.LastName, loadedStudent.LastName);
                Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
                Assert.Equal(student.Grades.Count, loadedStudent.Grades.Count);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Fact]
        public void Serialize_InvalidStudent_ThrowsValidationException()
        {
            var invalidStudent = new Student
            {
                FirstName = "", 
                LastName = "Doe",
                BirthDate = new DateTime(2000, 1, 1),
                Grades = new List<Subject>()
            };
            Assert.Throws<ArgumentException>(() => StudentJsonSerializer.Serialize(invalidStudent));
        }

        [Fact]
        public void Deserialize_InvalidJson_ThrowsValidationException()
        {
            string invalidJson = @"{
                ""firstName"": """",
                ""lastName"": ""Doe"",
                ""birthDate"": ""2000-01-01"",
                ""grades"": []
            }";
            Assert.Throws<ArgumentException>(() => StudentJsonSerializer.Deserialize(invalidJson));
        }
    }
}
