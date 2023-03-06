namespace JwtWebApi.tables
{
    public class Hero
    {
        public string? Name { get; set; }
        public string? Ability { get; set; }
        public string? TrainerName { get; set; }

        public int? Id { get; set; }
        public int? FirstDayHeroTrained { get; set; }
        public double? StartingPower { get; set; }

        public double? CurrentPower { get; set; }
        public string? SuitColors { get; set; }

        public string? LastTimeHeroTrained { get; set; }

    }
}
