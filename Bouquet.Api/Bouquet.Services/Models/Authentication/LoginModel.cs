using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.Services.Models.Authentication
{
    public class LoginModel
    {
        /// <summary>
        /// Emaul
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string? Password { get; set; }
    }

}
