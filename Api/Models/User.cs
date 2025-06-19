using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.Models
{

    public class User
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
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
