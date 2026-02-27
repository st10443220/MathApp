using System;

namespace MathApp;

public class Error
{
    public int code { get; set; }
    public string message { get; set; }
    public List<Error> errors { get; set; }
}
