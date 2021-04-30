﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Data
{
    public class PaymentDbContext : IdentityDbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }


    }
}
