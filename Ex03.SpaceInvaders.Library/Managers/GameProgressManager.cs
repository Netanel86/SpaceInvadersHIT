namespace Ex03.SpaceInvaders.Library.GameServices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.Managers;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Sprites;
    using Ex03.SpaceInvaders.Library.Screens;
    
    public class GameProgressManager : GameService, IInterceptionManager, IPlayerManager
    {
        private readonly List<IPlayer> r_Players;
        private readonly Dictionary<PlayerIndex, ScoreBoard> r_ScoreBoard;
        private readonly Dictionary<PlayerIndex, SoulSprite> r_SoulSprites;
        
        private int m_Level;
        private IScreenManager m_ScreenManager;
        private IPlayScreen m_PlayScreen;
        private IGameOverScreen m_GameOverScreen;

        ////////////////////////////////////////////////////
        private IAudioManager m_AudioManager;
        ////////////////////////////////////////////////////

        public GameProgressManager(Game i_Game)
            : base(i_Game)
        {
            r_SoulSprites = new Dictionary<PlayerIndex, SoulSprite>();
            r_ScoreBoard = new Dictionary<PlayerIndex, ScoreBoard>();
            r_Players = new List<IPlayer>();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Level = 0;
            m_ScreenManager = this.Game.Services.GetService(typeof(IScreenManager)) as IScreenManager;
            m_GameOverScreen = this.Game.Services.GetService(typeof(IGameOverScreen)) as IGameOverScreen;
            m_PlayScreen = this.Game.Services.GetService(typeof(IPlayScreen)) as IPlayScreen;
            IInvaderManager invaderMngr = this.Game.Services.GetService(typeof(IInvaderManager)) as IInvaderManager;
            invaderMngr.OutOfScreenBoundries += () => gameOver(eGameEndedScenario.AllPlayersAreDead);
            invaderMngr.GroupCountZero += invaderManager_GroupCountZero;

            ////////////////////////////////////////////////////
            m_AudioManager = Game.Services.GetService(typeof(IAudioManager)) as IAudioManager;
            ////////////////////////////////////////////////////
        }
        
        public void AddPlayer(IPlayer i_Player)
        {
            SoulSprite soulSprite = new SoulSprite(this.Game, @"Sprites\Ships_64x32");
            soulSprite.PlayerIdx = i_Player.PlayerIdx;
            soulSprite.NumberOfSouls = i_Player.Souls;
            soulSprite.SourceRectangle = i_Player.SourceRectangle;
            m_PlayScreen.Add(soulSprite);
            r_SoulSprites.Add(i_Player.PlayerIdx, soulSprite);

            ScoreBoard score = new ScoreBoard(this.Game, "Arial");
            score.PlayerIdx = i_Player.PlayerIdx;
            score.TintColor = i_Player.PlayerIdx == PlayerIndex.One ? Color.Blue : Color.Green;
            m_PlayScreen.Add(score);
            r_ScoreBoard.Add(i_Player.PlayerIdx, score);

            i_Player.SoulsChanged += player_SoulsChanged;
            i_Player.Disposed += player_Disposed;
            i_Player.EnabledChanged += player_EnabledChanged;
            r_Players.Add(i_Player);
        }

        public void AttachInterceptable(IInterceptable i_Interceptable)
        {
            i_Interceptable.Intercepted += interceptable_Intercepted;
            i_Interceptable.Disposed += interceptable_Disposed;
        }
        
        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IInterceptionManager), this);
            this.Game.Services.AddService(typeof(IPlayerManager), this);
        }

        private void invaderManager_GroupCountZero()
        {
            m_Level++;
            m_ScreenManager.SetCurrentScreen(new LevelTransitionScreen(this.Game, m_Level));

            ////////////////////////////////////////////////////
            m_AudioManager.Play("LevelWin");
            ////////////////////////////////////////////////////

            m_PlayScreen.ResetNextLevel(m_Level);
        }

        private void player_Disposed(object sender, EventArgs e)
        {
            IPlayer player = sender as IPlayer;
            player.SoulsChanged -= player_SoulsChanged;
            player.Disposed -= player_Disposed;
            player.EnabledChanged -= player_EnabledChanged;
            r_Players.Remove(player);
            r_SoulSprites.Remove(player.PlayerIdx);
        }

        private void player_SoulsChanged(object sender, EventArgs e)
        {
            IPlayer player = sender as IPlayer;

            r_SoulSprites[player.PlayerIdx].NumberOfSouls = player.Souls;
        }

        private void player_EnabledChanged(object sender, EventArgs e)
        {
            if (!checkIfAnyPlayersLeft())
            {
                ////////////////////////////////////////////////////
                m_AudioManager.Play("GameOver");
                ////////////////////////////////////////////////////

                gameOver(eGameEndedScenario.AllPlayersAreDead);
            }
        }

        private void interceptable_Intercepted(object i_Sender, EventArgs i_Args)
        {
            IInterceptable intercepted = i_Sender as IInterceptable;
            if (intercepted != null)
            {
                if (intercepted is Enemy)
                {
                    ////get the player index representing who fired the bullet
                    PlayerIndex playerIdx = (i_Args as InterceptionEventArgs).BulletSource;
                    r_ScoreBoard[playerIdx].UpdateScoreBoard(intercepted.Score);
                }
                else
                {
                    IPlayer player = intercepted as IPlayer;
                    if (player != null)
                    {
                        r_ScoreBoard[player.PlayerIdx].UpdateScoreBoard(intercepted.Score);
                    }
                }
            }
        }

        private void interceptable_Disposed(object sender, EventArgs e)
        {
            IInterceptable notifier = sender as IInterceptable;
            if (notifier != null)
            {
                notifier.Disposed -= this.interceptable_Disposed;
                notifier.Intercepted -= this.interceptable_Intercepted;
            }
        }

        private bool checkIfAnyPlayersLeft()
        {
            bool isAtleastOneAlive = false;
            foreach (IPlayer player in r_Players)
            {
                if (player.Enabled)
                {
                    isAtleastOneAlive = true;
                    break;
                }
            }

            return isAtleastOneAlive;
        }

        private void gameOver(eGameEndedScenario i_Scenario)
        {
            string gameEndMessage = null;
            StringBuilder builder = new StringBuilder();

            switch (i_Scenario)
            {
                case eGameEndedScenario.AllPlayersAreDead: 
                    gameEndMessage = "You Were Crushed!!";
                    builder.Append("You Were Defeated By The Invader's!\n");
                    break;

                case eGameEndedScenario.AllEnemiesAreDead: 
                    gameEndMessage = "You Are Triumphant!!";
                    builder.Append("You Have Crushed The Invasion!\n");
                    break;
            }

            if (r_ScoreBoard.Count > 1 && r_ScoreBoard[PlayerIndex.One].TargetScore == r_ScoreBoard[PlayerIndex.Two].TargetScore)
            {
                builder.AppendFormat("Game Ended With A Tie Of {0} Points!", r_ScoreBoard[PlayerIndex.One].TargetScore);
            }
            else
            {
                var sortedScores = from entry in r_ScoreBoard
                                   orderby entry.Value.TargetScore descending
                                   select new { PlayerIndex = entry.Value.PlayerIdx, Score = entry.Value.TargetScore };

                int idx = 1;
                foreach (var score in sortedScores)
                {
                    if (idx == 1)
                    {
                        builder.AppendFormat("Player {0} Won The Game With {1} Points.", score.PlayerIndex, score.Score);
                    }
                    else
                    {
                        builder.AppendFormat(
                            "\nPlayer {0} Reached {1}{2} Place With {3} Points.",
                            score.PlayerIndex,
                            idx,
                            idx == 2 ? "nd" : idx == 3 ? "rd" : "th",
                            score.Score);
                    }

                    idx++;
                }
            }

            r_ScoreBoard.Clear();
            m_GameOverScreen.GameEndMessege = builder.ToString();
        }

        public enum eGameEndedScenario
        {
            AllEnemiesAreDead,
            AllPlayersAreDead
        }
    }
}
