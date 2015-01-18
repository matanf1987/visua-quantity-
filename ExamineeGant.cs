using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryWPF
{
    class ExamineeGant
    {
        public Dictionary<string, DailyGamesInformation> days = new Dictionary<string, DailyGamesInformation>();
        public string userName { get; set; }
        public string participantNumber { get; set; }
     	public string date{ get; set; }
	    public string day{ get; set; }
	    public string hour{ get; set; }
        public string robotGameTimeTotal{ get; set; }
        public string skyGameTimeTotal{ get; set; }
	    public string treasureMapGameTimeTotal{ get; set; }
	    public string treasureMapReverseGameTimeTotal{ get; set; }
	    public string castleGameTimeTotal{ get; set; }
        public string villageGameLocationTimeTotal { get; set; }
        public string villageGameIdentityTimeTotal { get; set; }
	    public string robotGameTimeLeft{ get; set; }
	    public string skyGameTimeLeft{ get; set; }
	    public string treasureMapGameTimeLeft{ get; set; }
	    public string treasureMapReverseGameTimeLeft{ get; set; }
        public string castleGameTimeLeft { get; set; }
        public string villageGameLocationTimeLeft { get; set; }
        public string villageGameIdentityTimeLeft { get; set; }

        public void AddDayToDictionary(string date, string day, string hour, string robotGameTimeTotal, string skyGameTimeTotal, string treasureMapGameTimeTotal, string treasureMapReverseGameTimeTotal, string castleGameTimeTotal, string villageGameLocationTimeTotal, string villageGameIdentityTimeTotal, string robotGameTimeLeft, string skyGameTimeLeft, string treasureMapGameTimeLeft, string treasureMapReverseGameTimeLeft, string castleGameTimeLeft, string villageGameLocationTimeLeft, string villageGameIdentityTimeLeft)
        {
            DailyGamesInformation dgi = new DailyGamesInformation();
            dgi.day = day;
            dgi.hour = hour;
            dgi.robotGameTimeTotal = robotGameTimeTotal;
            dgi.skyGameTimeTotal = skyGameTimeTotal;
            dgi.treasureMapGameTimeTotal = treasureMapGameTimeTotal;
            dgi.treasureMapReverseGameTimeTotal = treasureMapReverseGameTimeTotal;
            dgi.castleGameTimeTotal = castleGameTimeTotal;
            dgi.villageGameLocationTimeTotal = villageGameLocationTimeTotal;
            dgi.villageGameIdentityTimeTotal = villageGameIdentityTimeTotal;
            dgi.robotGameTimeLeft = robotGameTimeLeft;
            dgi.skyGameTimeLeft = skyGameTimeLeft;
            dgi.treasureMapGameTimeLeft = treasureMapGameTimeLeft;
            dgi.treasureMapReverseGameTimeLeft = treasureMapReverseGameTimeLeft;
            dgi.castleGameTimeLeft = castleGameTimeLeft;
            dgi.villageGameLocationTimeLeft = villageGameLocationTimeLeft;
            dgi.villageGameIdentityTimeLeft = villageGameIdentityTimeLeft;
            days[date] = dgi;
        }
    }

    class DailyGamesInformation
    {

      
        public string day { get; set; }
        public string hour { get; set; }
        public string robotGameTimeTotal { get; set; }
        public string skyGameTimeTotal { get; set; }
        public string treasureMapGameTimeTotal { get; set; }
        public string treasureMapReverseGameTimeTotal { get; set; }
        public string castleGameTimeTotal { get; set; }
        public string villageGameLocationTimeTotal { get; set; }
        public string villageGameIdentityTimeTotal { get; set; }
        public string robotGameTimeLeft { get; set; }
        public string skyGameTimeLeft { get; set; }
        public string treasureMapGameTimeLeft { get; set; }
        public string treasureMapReverseGameTimeLeft { get; set; }
        public string castleGameTimeLeft { get; set; }
        public string villageGameLocationTimeLeft { get; set; }
        public string villageGameIdentityTimeLeft { get; set; }
    }
}
