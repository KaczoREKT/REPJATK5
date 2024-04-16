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
    public IActionResult GetAnimals()
    {
        
        // Otwieramy połączenie

        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();



            //Definiujemy Commanda
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Animal";


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
            command.CommandText = "INSERT INTO Animal VALUES (@animalName, @Description,'','')";
            command.Parameters.AddWithValue("@animalName", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Descritpion);
        
        
            //Otwieramy
            command.ExecuteNonQuery();
            return Created("", null);
        }
        
    }
}