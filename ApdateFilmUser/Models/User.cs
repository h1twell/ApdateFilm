using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{

    public class User
    {
        public int id { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        // Конструктор для удобства инициализации

        public User() { }
        public User(string surname, string name, string email, string password, DateTime birthday)
        {
            Surname = surname;
            Name = name;
            Email = email;
            Password = password;
            Birthday = birthday;
        }

        public User (string email, string password)
        {
            Email = email;
            Password = password;
        }

        // Переопределение метода ToString для удобства вывода информации о пользователе
        public override string ToString()
        {
            return $"Surname: {Surname}, Name: {Name}, Email: {Email}, Password: {Password}, Birthday: {Birthday}";
        }
    }
}
