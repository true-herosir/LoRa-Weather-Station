using System.Windows.Input;

namespace WeatherThingy.Sources.ViewModels
{

    public partial class AboutViewModel : ObservableObject
    {
        public ObservableCollection<TeamMember> TeamMembers { get; } = new();
        public ICommand EmailCommand { get; }
        public ICommand LinkedInUrlCommand { get; }
        public ICommand GitHubUrlCommand { get; }

        public AboutViewModel()
        {
            LoadTeamMembers();
            EmailCommand = new AsyncRelayCommand<string>(email => Launcher.Default.OpenAsync(new Uri($"mailto:{email}")));
            LinkedInUrlCommand = new AsyncRelayCommand<string>(link => Launcher.Default.OpenAsync(new Uri(link)));
            GitHubUrlCommand = new AsyncRelayCommand<string>(link => Launcher.Default.OpenAsync(new Uri(link)));
        }

        private void LoadTeamMembers()
        {
            TeamMembers.Add(new TeamMember
            {
                Name = "Dung Phan",
                Role = "Scrum Master - Software Developer",
                Education = "Saxion University of Applied Sciences",
                ImageUrl = "https://media.licdn.com/dms/image/v2/D4E35AQFvHQ7d8pro8Q/profile-framedphoto-shrink_400_400/profile-framedphoto-shrink_400_400/0/1734102169720?e=1736769600&v=beta&t=R6vwftS2kXTNCY1PqzdWKNHwaGMt9__OjjyqirDFDZk",
                Email = "546821@student.saxion.nl",
                LinkedInUrl = "https://www.linkedin.com/in/dungpv294/",
                GitHubUrl = "https://github.com/dungphan294"
            });

            TeamMembers.Add(new TeamMember
            {
                Name = "Karolina Gogolin",
                Role = "Software Developer",
                Education = "Saxion University of Applied Sciences",
                ImageUrl = "https://media.licdn.com/dms/image/v2/D4D35AQHgDX2156gBuw/profile-framedphoto-shrink_400_400/profile-framedphoto-shrink_400_400/0/1734000780612?e=1736769600&v=beta&t=08D3IPncaWwOAU1TLGMLiHtKb1m2JuesZdQSw7tMEww",
                Email = "543772@student.saxion.nl",
                LinkedInUrl = "https://www.linkedin.com/in/karolina-gogolin-8a9935330/",
                GitHubUrl = "https://github.com/KarolinaGogolin"
            });

            TeamMembers.Add(new TeamMember
            {
                Name = "Mikołaj Materka",
                Role = "Software Developer",
                Education = "Saxion University of Applied Sciences",
                ImageUrl = "https://media.licdn.com/dms/image/v2/D4E03AQH_CcY81qD51A/profile-displayphoto-shrink_400_400/B4EZPAywL_HwAg-/0/1734106351045?e=1741824000&v=beta&t=1olHyjS6z2kS28tlLp1HEb3u3UrdyJWMvUAoSEoFkTU",
                Email = "548471@student.saxion.nl",
                LinkedInUrl = "https://www.linkedin.com/in/miko%C5%82aj-materka-644b6233a/",
                GitHubUrl = "https://github.com/true-herosir"
            });

            TeamMembers.Add(new TeamMember
            {
                Name = "Peter Samaan",
                Role = "Software Developer",
                Education = "Saxion University of Applied Sciences",
                ImageUrl = "https://media.licdn.com/dms/image/v2/D4E03AQHX6lO1ZmW6Ug/profile-displayphoto-shrink_400_400/B4EZOxSKAdHkAg-/0/1733846147652?e=1741824000&v=beta&t=BGQwdXzz1nE5irY6qxoP87aAINK95LNG-mbIIkU-qYc",
                Email = "545727@student.saxion.nl",
                LinkedInUrl = "https://www.linkedin.com/in/peter-samaan-39870729a/",
                GitHubUrl = "https://github.com/PeterSamaan"
            });
        }
    }

    public partial class TeamMember
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Education { get; set; }
        public string? ImageUrl { get; set; }
        public string? Email { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
    }

}
