using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Rol : IdentityRole<int>
    {
        public Rol() : base()
        {

        }

        public Rol(string name) : base(name)
        {
          
        }
        public int Id { get; set; }
        [Display(Name = "fdgdf")]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        public override string NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
    }
}
