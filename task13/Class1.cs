using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace task13
{
    public class Subject
    {
        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Range(1, 10, ErrorMessage = "Grade must be between 1 and 10")]
        public int Grade { get; set; }
    }

    public class Student
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; }

        [JsonPropertyName("birthDate")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime BirthDate { get; set; }

        [ValidateEnumerable(ErrorMessage = "Grades list contains invalid subjects")]
        public List<Subject> Grades { get; set; }
    }
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), Format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateEnumerableAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IEnumerable<object> enumerable)
            {
                var results = new List<ValidationResult>();
                foreach (var item in enumerable)
                {
                    var context = new ValidationContext(item);
                    Validator.TryValidateObject(item, context, results, true);
                }
                return results.Count == 0;
            }
            return true;
        }
    }

    public static class StudentJsonSerializer
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            IgnoreNullValues = true,
            Converters = { new CustomDateTimeConverter() }
        };

        public static string Serialize(Student student)
        {
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(student, new ValidationContext(student), validationResults, true))
            {
                throw new ArgumentException($"Validation failed: {string.Join(", ", validationResults.Select(v => v.ErrorMessage))}");
            }

            return JsonSerializer.Serialize(student, _options);
        }

        public static Student Deserialize(string json)
        {
            var student = JsonSerializer.Deserialize<Student>(json, _options);
            
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(student, new ValidationContext(student), validationResults, true))
            {
                throw new ArgumentException($"Validation failed: {string.Join(", ", validationResults.Select(v => v.ErrorMessage))}");
            }

            return student;
        }

        public static void SaveToFile(Student student, string filePath)
        {
            string json = Serialize(student);
            File.WriteAllText(filePath, json);
        }

        public static Student LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return Deserialize(json);
        }
    }
}