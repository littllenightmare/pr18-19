using System;
using System.Collections.Generic;

namespace pr18_19.database;

public partial class Asstoy
{
    public string Toy { get; set; } = null!;

    public decimal Cost { get; set; }

    public int Kolvo { get; set; }

    public int Age { get; set; }

    public string Fabrica { get; set; } = null!;

    public string City { get; set; } = null!;
}
