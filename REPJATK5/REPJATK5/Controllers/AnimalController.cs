using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using REPJATK5.Models;
using REPJATK5.Models.DTOs;

namespace REPJATK5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        string[] allowed = { "name", "description", "category", "area" };
        if (!allowed.Contains(orderBy))
        {
            orderBy = "name";
        }

        // Otwieramy połączenie

        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();


            //Definiujemy Commanda
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy}";


            // Wykonanie Commanda


            var reader = command.ExecuteReader();
            List<Animal> animals = new List<Animal>();

            int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
            int nameOrdinal = reader.GetOrdinal("Name");
            while (reader.Read())
            {
                animals.Add(new Animal()
                {
                    IdAnimal = reader.GetInt32(idAnimalOrdinal),
                    Name = reader.GetString(nameOrdinal)
                });
            }

            return Ok(animals);
        }
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            //Definiujemy Commanda
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO Animal VALUES (@animalName, @Description,@Category,@Area)";
            command.Parameters.AddWithValue("@animalName", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Description);
            command.Parameters.AddWithValue("@Category", animal.Category);
            command.Parameters.AddWithValue("@Area", animal.Area);


            //Otwieramy
            command.ExecuteNonQuery();
            return Created("", null);
        }
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, AddAnimal animal)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            // Definiujemy Commanda
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText =
                "UPDATE Animal SET Name = @animalName, Description = @Description WHERE IdAnimal = @idAnimal";
            command.Parameters.AddWithValue("@animalName", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Description);
            command.Parameters.AddWithValue("@Category", animal.Category);
            command.Parameters.AddWithValue("@Area", animal.Area);
            command.Parameters.AddWithValue("@idAnimal", idAnimal);

            // Otwieramy
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                // Jeśli brak zmienionych wierszy, zwróć NotFound
                return NotFound();
            }

            return NoContent();
        }
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            // Definiujemy Commanda
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM Animal WHERE IdAnimal = @idAnimal";
            command.Parameters.AddWithValue("@idAnimal", idAnimal);

            // Otwieramy
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                // Jeśli brak zmienionych wierszy, zwróć NotFound
                return NotFound();
            }

            return NoContent();
        }
    }
}