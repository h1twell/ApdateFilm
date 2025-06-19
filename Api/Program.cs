using ApdateFilmUser.Models;
using Api.Services.API;
using Api.Servieces;

ApiClient.Initialize("http://localhost:8000/");

User user;

//user = new User("ApiConsole", "Console", "Console@api.com", "12345678", new DateTime(2007, 08, 02));
//var userResponse = await AuthServiec.AuthAsync(user);
//Console.WriteLine(userResponse);

user = new User("Console@api.com", "12345678");
user = await AuthServiec.LoginAsync(user);
Console.WriteLine(user);

var mediaList = await MediaServiec.GetMediaAsync();

Console.WriteLine($"{mediaList} and {mediaList.Count}");