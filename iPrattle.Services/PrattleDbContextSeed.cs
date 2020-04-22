using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iPrattle.Services
{
    public class PrattleDbContextSeed
    {
        public static void SeedAsync(PrattleDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Log.Error($"Error while seeding DB; {ex.Message}");
            }
        }
    }
}
