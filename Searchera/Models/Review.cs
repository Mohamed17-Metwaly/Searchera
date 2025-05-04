#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Searchera.Models;

public partial class Review
{
    public int Id { get; set; }
    [Required(ErrorMessage ="The Review Text is Requierd!")]
    [MaxLength(100,ErrorMessage ="This is Long text")]
    [MinLength(10, ErrorMessage = "This is small text")]
    public string Reviewtext { get; set; }
    [Required(ErrorMessage = "The Review Text is Requierd!")]
    public int Rating { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [ForeignKey("Company")]
    public int CompanyID { get; set; }

    public virtual Company? Company { get; set; }

    public virtual User? User { get; set; }
}