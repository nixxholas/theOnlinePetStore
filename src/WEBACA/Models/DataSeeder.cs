using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;


namespace WEBACA.Models
{
    public static class DataSeeder
    {
        public static async void SeedData(this IApplicationBuilder app)
        {
            var db = app.ApplicationServices.GetService<ApplicationDbContext>();

            db.Database.Migrate();

            Visibility Visible, VisibleIgnored, Hidden;

            VisibleIgnored = new Visibility()
            {
                VisibilityName = "Visible (Ignoring Start Date and End Date)"
            };
            db.Visibilities.Add(VisibleIgnored);

            Visible = new Visibility()
            {
                VisibilityName = "Visible (with Start Date and End Date)"
            };
            db.Visibilities.Add(Visible);

            Hidden = new Visibility()
            {
                VisibilityName = "Hidden"
            };
            db.Visibilities.Add(Hidden);
            

            db.SaveChanges();
            return;
        }//End of SeedData() method
    }
}
