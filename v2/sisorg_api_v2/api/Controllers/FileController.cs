using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;
using System;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics.Metrics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly SisorgContext _context;

        public FileController(FileService fileService, SisorgContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        [HttpGet("read/{ID}")]
        public async Task<IActionResult> GetData(int ID)
        {
            try
            {
                var marker = await _context.Markers
                 .Where(m => m.ID == ID)
                 .Select(m => new
                 {
                    m.ID,
                    m.Count,
                    m.Timestamp,
                    Rows = m.Rows.Select(c => new
                    {
                        Name = c.Name,
                        Value = c.Value,
                        Color = c.Color
                    })
                 })
                 .FirstOrDefaultAsync();

                if (marker == null)
                {
                    return NotFound("The Register does not exist");
                }
                else
                {  
                    return Ok(marker);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            try
            {
                // Raw data
                var resultRaw = _fileService.ReadFile(file);

                if (resultRaw.Successfull)
                {
                    // Separate in rows
                    string[] countries = resultRaw.Data.Split("\r\n");

                    // Filter empty fields 
                    countries = countries.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();

                    // Marker data
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

                    // Instace of Marker
                    Marker marker = new Marker(countries.Length, timeStamp, countryList);

                    // Save on server
                    _context.Markers.Add(marker);
                    await _context.SaveChangesAsync();


                    // Response with the marker with out the 
                    var result = new
                    {
                        ID = marker.ID,
                        Count = marker.Count,
                        Timestamp = marker.Timestamp,
                        Rows = marker.Rows.Select(c => new
                        {
                            Name = c.Name,
                            Value = c.Value,
                            Color = c.Color, 
                        }),        
                    };

                    return Ok(result);
                }
                else
                {
                    return BadRequest(resultRaw.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete/{ID}")]
        public async Task<IActionResult> DeleteData(int ID)
        {
            try
            {
                var markItem = await _context.Markers.FindAsync(ID);

                if (markItem == null)
                {
                    return NotFound();
                }

                _context.Markers.Remove(markItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
