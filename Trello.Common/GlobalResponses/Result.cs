using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


public class Result
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }

    public Result(List<string> errors)
    {
        Errors = errors;
        IsSuccess = false;
    }

    public Result()
    {
        IsSuccess = true;
        Errors = [];
    }
}