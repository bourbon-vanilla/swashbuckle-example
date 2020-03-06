using System;
using System.ComponentModel.DataAnnotations;

namespace SwashbuckleExample.Dto
{
    /// <summary>
    /// Wether forecast transfer object.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The Date.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The temperature in celsius degrees.
        /// </summary>
        [Required]
        public int TemperatureC { get; set; }

        /// <summary>
        /// The temperature in farenheit degrees.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// The summary.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Summary { get; set; }
    }
}
