﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Food
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ImagePath { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Type { get; set; } 
        }
   

    
}
