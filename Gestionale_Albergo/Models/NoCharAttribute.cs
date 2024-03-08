using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Gestionale_Albergo.Models
{
    public class NoCharAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // Conta il numero di occorrenze di caratteri alfabetici nella stringa da confrontare
            int nr = Regex.Matches("stringadaconfrontare", @"[a-zA-Z]").Count;
            if (nr > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}