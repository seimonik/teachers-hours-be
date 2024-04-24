using System.ComponentModel.DataAnnotations;

namespace TH.S3Client;

public class S3Options
{
    [Required]
    public string Host { get; set; } = null!;
    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Bucket { get; set; } = null!;
}
