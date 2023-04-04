namespace JwtWebApi.tables
{
    public class Hero
    {

        public string? Name { get; set; }
        public string? Ability { get; set; }
        public int? Id { get; set; }
        public int? FirstDayHeroTrained { get; set; }
        public string? SuitColors { get; set; }
        public double? StartingPower { get; set; }
        public double? CurrentPower { get; set; }
        public string? LastTimeHeroTrained { get; set; }
        public int? AmountOfTimeHeroTrained { get; set; }
        public string? TrainerName { get; set; }

        public Hero(string name, string ability, int firstDayHeroTrained, string suitColors, double startingPower, double currentPower, string lastTimeHeroTrained, int herosTrainingTimes,string trainerName)
        {
            Name = name;
            Ability = ability;
            FirstDayHeroTrained = firstDayHeroTrained;
            SuitColors = suitColors;
            StartingPower = startingPower;
            CurrentPower = currentPower;
            LastTimeHeroTrained = lastTimeHeroTrained;
            AmountOfTimeHeroTrained = herosTrainingTimes;
            TrainerName = trainerName;
        }
        public Hero() { }


    }
}
