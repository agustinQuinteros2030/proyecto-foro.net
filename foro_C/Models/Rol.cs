using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using foro_C.HelpersDataAnotattions;

namespace foro_C.Models
{
    public class Rol : IdentityRole<int>
    {
        // public int Id { get; set; }

        #region Constructores
        public Rol() : base() { }
        public Rol(string name) : base(name) { }

        #endregion

        #region Propiedades
        [Display(Name = "Nombre")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        public override string NormalizedName
        {
            get => base.NormalizedName;
            set => base.NormalizedName = value;
        }
        #endregion
    }
}
