using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Game
    {
        public Game()
        {
            GamePlayers = new List<GamePlayer>();
        }
        public int Id { get; set; }
        public int Round { get; set; }
        public DateTimeOffset? SrartTime { get; set; }
        public List<GamePlayer> GamePlayers { get; set; }
    }
    public class GamePlayer
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public Player Player { get; set; }
        public Game Game { get; set; }
    }
}
