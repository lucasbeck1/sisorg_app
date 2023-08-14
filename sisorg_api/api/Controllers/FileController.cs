using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;
using System;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics.Metrics;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }


        [HttpGet("{ID}")]
        public IActionResult GetData(string ID)
        {
            try
            {
                string filePath = "./Files/log_" + ID + ".txt";
                string fileContent;

                if (System.IO.File.Exists(filePath))
                {
                    fileContent = System.IO.File.ReadAllText(filePath);
                    return Ok(fileContent);
                }
                else
                {
                    return BadRequest("File does not exist"); ;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            try
            {
                // Raw data
                var result = _fileService.ReadFile(file);

                if (result.success)
                {
                    // Separate in rows
                    string[] countries = result.data.Split("\r\n");

                    // Filter empty fields 
                    countries = countries.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();

                    // Marker data
                    int id = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int countriesLength = countries.Length;
                    DateTime timeStamp = DateTime.Now;
                    List<Country> countryList = new List<Country>();

                    // Instace of country
                    foreach (string row in countries)
                    {
                        string[] countryData = row.Split("#");
                        string countryName = countryData[0];
                        decimal countryValue = decimal.Parse(countryData[1]);
                        string countryColor = countryData[2];

                        Country country = new Country(countryName, countryValue, countryColor);
                        countryList.Add(country);
                    }

                    List<string> rows = new List<string> { };
                    foreach (Country row in countryList)
                    {
                        rows.Add(row.ToString());
                    }

                    // Instace of Marker
                    Marker marker = new Marker(id, countries.Length, timeStamp, countryList);


                    // Save on server
                    using (StreamWriter archivo = new StreamWriter("./Files/log_" + id + ".txt"))
                    {
                        archivo.WriteLine("ID: " + marker.ID);
                        archivo.WriteLine("Count: " + marker.Count);
                        archivo.WriteLine("Timestamp: " + marker.Timestamp);
                        archivo.WriteLine("Rows: " + string.Join(",", rows));
                    }

                    return Ok(marker);
                }
                else
                {
                    return BadRequest(result.message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete/{ID}")]
        public IActionResult DeleteData(string ID)
        {
            try
            {
                string filePath = "./Files/log_" + ID + ".txt";

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok("File deleted succesfull");
                }
                else
                {
                    return BadRequest("File does not exist"); ;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
