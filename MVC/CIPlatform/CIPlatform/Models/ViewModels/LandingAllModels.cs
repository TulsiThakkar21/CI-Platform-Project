namespace CIPlatform.Models.ViewModels
{
    public class LandingAllModels
    {
        public City City { get; set; }

        public Country Country { get; set; }

        public Mission Mission { get; set; }

        public MissionTheme MissionTheme { get; set; }

        public MissionSkill MissionSkill { get; set; }
        
        public FavoriteMission FavoriteMission { get; set; }

        public bool IsFavorite { get; set; }
        public User User { get; set; }

       // public List<MissionRating> missionRatings { get; set; }

    

        public List<User> UsersList { get; set; }

       

        

    }

    public class CheckBoxViewModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
}
